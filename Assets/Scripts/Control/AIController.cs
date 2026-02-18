using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTimer = 5f;
        [SerializeField] PatrolPath patrolPath;

        private Fighter fighter;
        private Mover mover;
        private Health health;
        private GameObject player;

        private Vector3 guardPosition;
        private int currentWaypointIndex = 0;
        private Quaternion guardRotation;
        private float timeSinceLastSawPlayer = Mathf.Infinity;

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
            guardRotation = transform.rotation;
        }


        private void Update()
        {
            if (health.IsDead) { return; }


            if (IsInAttackRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTimer)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            mover.StartMoving(nextPosition);

            //if (Vector3.Distance(transform.position, guardPosition) < 0.2f)
            //{
            //    transform.rotation = Quaternion.RotateTowards(
            //        transform.rotation,
            //        guardRotation,
            //        360f * Time.deltaTime
            //    );
            //}
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < 1f;
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