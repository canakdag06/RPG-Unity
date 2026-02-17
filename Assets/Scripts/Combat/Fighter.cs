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

        private Transform target;
        private Mover mover;
        private Animator animator;

        private const string attackTrigger = "attack";

        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!IsTargetInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.transform;

        }

        public void Cancel()
        {
            this.target = null;
        }

        // Animation Event
        void Hit()
        {
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private bool IsTargetInRange()
        {
            float distance = (target.position - transform.position).sqrMagnitude;

            if (distance < weaponRange * weaponRange)
                return true;
            else
                return false;
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > attackCooldown)
            {
                // This will trigger the Hit() event.
                animator.SetTrigger(attackTrigger);
                timeSinceLastAttack = 0f;
            }
        }

    }

}