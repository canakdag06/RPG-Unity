using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private const string ForwardSpeed = "forwardSpeed";

    void Update()
    {
        UpdateAnimator();
    }

    public void MoveTo(Vector3 position)
    {
        agent.destination = position;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat(ForwardSpeed, speed);
    }
}
