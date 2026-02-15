using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2.0f;

        private Transform target;
        private Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
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
                mover.Stop();
            }
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.transform;
        }

        public void CancelAttack()
        {
            this.target = null;
        }

        private bool IsTargetInRange()
        {
            float distance = (target.position - transform.position).sqrMagnitude;

            if (distance < weaponRange * weaponRange)
                return true;
            else
                return false;
        }
    }

}