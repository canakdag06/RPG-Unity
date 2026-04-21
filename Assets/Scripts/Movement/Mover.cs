using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] float speed;


        private const string ForwardSpeed = "forwardSpeed";


        private Health health;

        private void Awake()
        {
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

        public void StartMoving(Vector3 destination, float multiplier)
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}