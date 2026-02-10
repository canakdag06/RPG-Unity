using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    Ray lastRay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            lastRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);

        agent.SetDestination(target.transform.position);
    }
}
