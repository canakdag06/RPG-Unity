using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;


namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float health = -1f;

        public event Action OnDie;
        public event Action<float> OnHealthChanged;

        public bool IsDead => isDead;

        private const string dieTrigger = "die";

        private bool isDead = false;

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
            if(health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject attacker, float damage)
        {
            health = Mathf.Max(health - damage, 0f);
            OnHealthChanged?.Invoke(GetHealthPercentage());

            if (health == 0f)
            {
                Die();
                AwardEXP(attacker);
            }
        }

        public float GetHealthPercentage()
        {
            return (health / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
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
            float newHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            if (newHealth > health)
            {
                health = newHealth;
                OnHealthChanged?.Invoke(GetHealthPercentage());
            }
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;

            if (health <= 0f)
            {
                Die();
            }
        }
    }
}