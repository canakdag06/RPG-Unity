using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2.0f;

        private Transform target;
        private Mover mover;
        private Animator animator;

        private const string attackTrigger = "attack";

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (target == null) return;

            if (!IsTargetInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                Attack();
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

        }

        private bool IsTargetInRange()
        {
            float distance = (target.position - transform.position).sqrMagnitude;

            if (distance < weaponRange * weaponRange)
                return true;
            else
                return false;
        }

        private void Attack()
        {
            animator.SetTrigger(attackTrigger);
        }

    }

}