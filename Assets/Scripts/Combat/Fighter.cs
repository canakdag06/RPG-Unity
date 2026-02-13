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
            if (target != null)
            {
                float distanceToTarget = (target.position - transform.position).sqrMagnitude;

                if (distanceToTarget < weaponRange * weaponRange)
                {
                    mover.Stop();
                }
                else
                {
                    mover.MoveTo(target.position);
                }

            }
        }

        public void Attack(CombatTarget target)
        {
            this.target = target.transform;
        }
    }

}