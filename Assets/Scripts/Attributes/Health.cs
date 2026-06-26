using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;


namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {

        public event Action OnDie;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnTakeDamage;

        LazyValue<float> health;

        public bool IsDead => isDead;
        public float HealthPoints => health.value;

        private const string dieTrigger = "die";

        private bool isDead = false;


        private void Awake()
        {
            health = new LazyValue<float>(GetMaxHealthPoints);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelChanged += RefillHealthOnLevelUp;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelChanged -= RefillHealthOnLevelUp;
        }

        private void Start()
        {
            health.ForceInit();
            OnHealthChanged?.Invoke(GetHealthPercentage());
        }


        public void TakeDamage(GameObject attacker, float damage)
        {
            Debug.Log($"{gameObject.name} took {damage} damage from {attacker.name}");

            health.value = Mathf.Max(health.value - damage, 0f);
            OnHealthChanged?.Invoke(GetHealthPercentage());
            OnTakeDamage?.Invoke(damage);

            if (health.value == 0f)
            {
                Die();
                AwardEXP(attacker);
            }
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPercentage()
        {
            return (health.value / GetMaxHealthPoints()) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger(dieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();

            OnDie.Invoke();
        }

        private void AwardEXP(GameObject attacker)
        {
            Experience experience = attacker.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainEXP(GetComponent<BaseStats>().GetStat(Stat.EXPReward));
        }

        private void RefillHealthOnLevelUp(int newLevel)
        {
            float newHealth = GetMaxHealthPoints();
            if (newHealth > health.value)
            {
                health.value = newHealth;
                OnHealthChanged?.Invoke(GetHealthPercentage());
            }
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value <= 0f)
            {
                Die();
            }
        }
    }
}