using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        private Fighter fighter;
        private Mover mover;
        private Health health;
        private GameObject player;

        private Vector3 guardPosition;

        private const string playerTag = "Player";

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            player = GameObject.FindWithTag(playerTag);
            guardPosition = transform.position;
        }


        private void Update()
        {
            if (health.IsDead) { return; }


            if (IsInAttackRange() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                mover.StartMoving(guardPosition);
            }
        }

        private bool IsInAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}