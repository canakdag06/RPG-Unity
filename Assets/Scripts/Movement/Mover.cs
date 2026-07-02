using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        [SerializeField] private Animator animator;
        [SerializeField] float speed;
        [SerializeField] float maxNavPathLength = 40f;


        private const string ForwardSpeed = "forwardSpeed";

        private NavMeshAgent agent;
        private Health health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnDie += DisableNavMeshAgent;
        }


        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoving(Vector3 destination, float multiplier = 1f)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, multiplier);
        }

        public void MoveTo(Vector3 position, float multiplier)
        {
            agent.destination = position;
            agent.speed = speed * multiplier;
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private void OnDisable()
        {
            health.OnDie -= DisableNavMeshAgent;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(ForwardSpeed, speed);
        }

        private void DisableNavMeshAgent()
        {
            agent.enabled = false;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }
        

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            agent.enabled = false;
            transform.position = position.ToVector();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}