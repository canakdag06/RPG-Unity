using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] float damage;
        [SerializeField] float range;
        [SerializeField] bool isRightHanded = true;

        [SerializeField] Projectile projectile = null;

        public float Damage { get { return damage; } }
        public float Range { get { return range; } }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);

            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, handTransform);
            }
            else
            {
                Debug.LogWarning("No weapon prefab found for " + name);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                Debug.LogWarning("No animator override found for " + name);
            }
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }
    }
}