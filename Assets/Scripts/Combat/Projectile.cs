using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 1.0f;

        private void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogError("Target does not have a Collider.");
                return target.position;
            }

            return targetCollider.bounds.center;
        }
    }

}
