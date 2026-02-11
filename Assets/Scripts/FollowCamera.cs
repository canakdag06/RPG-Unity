using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
