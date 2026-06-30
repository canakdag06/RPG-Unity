using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float attackCooldown = 1.0f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        private Health target;
        private Mover mover;
        private Animator animator;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private const string attackTrigger = "attack";
        private const string stopAttackTrigger = "stopAttack";

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead) return;

            if (!IsTargetInRange())
            {
                if (!IsPlayingAttackAnimation())
                {
                    mover.MoveTo(target.transform.position, 1f);
                }
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            this.target = null;
            mover.Cancel();
            animator.ResetTrigger(attackTrigger);
            animator.SetTrigger(stopAttackTrigger);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            if (currentWeapon.value != null)
            {
                Destroy(currentWeapon.value.gameObject);
            }

            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHand, leftHand, animator);
        }

        public IEnumerable<float> GetModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.BonusDamagePercentage;
            }
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }


        private bool IsPlayingAttackAnimation()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        private bool IsTargetInRange()
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance < currentWeaponConfig.Range * currentWeaponConfig.Range)
                return true;
            else
                return false;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);

            if (timeSinceLastAttack > attackCooldown)
            {
                // This will trigger the Hit() event.
                animator.ResetTrigger(stopAttackTrigger);
                animator.SetTrigger(attackTrigger);
                timeSinceLastAttack = 0f;
            }
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }

}