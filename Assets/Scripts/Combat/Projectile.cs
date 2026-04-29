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
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                Debug.LogError("Target does not have a Collider.");
                return target.position;
            }

            return target.position + Vector3.up * targetCapsule.height / 1.5f;
        }
    }

}
