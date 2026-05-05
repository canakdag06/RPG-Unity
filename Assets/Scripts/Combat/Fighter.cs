using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackCooldown = 1.0f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] Weapon defaultWeaponType = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        private Health target;
        private Mover mover;
        private Animator animator;
        private Weapon currentWeaponType = null;
        private GameObject currentWeapon = null;

        private const string attackTrigger = "attack";
        private const string stopAttackTrigger = "stopAttack";

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            //EquipWeapon(defaultWeaponType);
            Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipWeapon(weapon);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead) return;

            if (!IsTargetInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
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
        public void EquipWeapon(Weapon weapon)
        {
            if(currentWeapon != null)
            {
                Destroy(currentWeapon);
            }

            currentWeaponType = weapon;
            currentWeapon = currentWeaponType.Spawn(rightHand, leftHand, animator);
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            if (currentWeaponType.HasProjectile())
            {
                currentWeaponType.LaunchProjectile(rightHand, leftHand, target);
            }
            else
            {
                target.TakeDamage(currentWeaponType.Damage);
            }
        }

        void Shoot()
        {
            Hit();
        }


        private bool IsTargetInRange()
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance < currentWeaponType.Range * currentWeaponType.Range)
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

    }

}