using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] float damage;
        [SerializeField] float bonusDamagePercentage;
        [SerializeField] float range;
        [SerializeField] bool isRightHanded = true;

        [SerializeField] Projectile projectile = null;

        public float Damage { get { return damage; } }
        public float BonusDamagePercentage { get { return bonusDamagePercentage; } }
        public float Range { get { return range; } }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);
            Weapon weaponInstance = null;

            if (weaponPrefab != null)
            {
                weaponInstance = Instantiate(weaponPrefab, handTransform);
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

            return weaponInstance;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject attacker, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, attacker, calculatedDamage);
        }

        public IEnumerable<float> GetModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return damage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return bonusDamagePercentage;
            }
        }
    }
}