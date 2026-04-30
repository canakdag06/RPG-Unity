using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private bool isHoming = false;
        private Health target;
        private float damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());

        }

        private void Update()
        {
            if (target == null) return;
            if(isHoming && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogError("Target does not have a Collider.");
                return target.transform.position;
            }

            return targetCollider.bounds.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if(target.IsDead) return;

            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
