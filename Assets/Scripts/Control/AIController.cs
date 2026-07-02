using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTimer = 5f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] float waypointTimer = 5f;
        [SerializeField] float patrolSpeedMultiplier = 0.5f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] PatrolPath patrolPath;

        private Fighter fighter;
        private Mover mover;
        private Health health;
        private GameObject player;

        LazyValue<Vector3> guardPosition;
        LazyValue<Quaternion> guardRotation;
        private int currentWaypointIndex = 0;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        private const string playerTag = "Player";

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag(playerTag);

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            guardRotation = new LazyValue<Quaternion>(GetGuardRotation);
        }

        private void OnEnable()
        {
            health.OnTakeDamage += OnDamageTaken;
        }

        private void OnDisable()
        {
            health.OnTakeDamage -= OnDamageTaken;
        }

        private void Start()
        {
            guardPosition.ForceInit();
            guardRotation.ForceInit();
        }


        private void Update()
        {
            if (health.IsDead) { return; }


            if (IsAggrevated() && fighter.CanAttack(player))
            {
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

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void OnDamageTaken(float damage)
        {
            Aggrevate();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private Quaternion GetGuardRotation()
        {
            return transform.rotation;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();

                if (timeSinceArrivedAtWaypoint > waypointTimer)
                {
                    mover.StartMoving(nextPosition, patrolSpeedMultiplier);
                }
            }
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

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}