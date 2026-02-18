using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float attackCooldown = 1.0f;
        [SerializeField] float weaponDamage = 5f;

        private Health target;
        private Mover mover;
        private Animator animator;

        private const string attackTrigger = "attack";
        private const string stopAttackTrigger = "stopAttack";

        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead) return;

            if (!IsTargetInRange())
            {
                mover.MoveTo(target.transform.position);
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
            animator.ResetTrigger(attackTrigger);
            animator.SetTrigger(stopAttackTrigger);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            target.TakeDamage(weaponDamage);
        }

        private bool IsTargetInRange()
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance < weaponRange * weaponRange)
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