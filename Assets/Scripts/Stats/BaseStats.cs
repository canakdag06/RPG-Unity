using GameDevTV.Utils;
using System;
using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] bool useModifiers;

        [SerializeField] ParticleSystem levelUpEffect;

        public event Action<int> OnLevelChanged;

        LazyValue<int> currentLevel;
        private Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.OnExpChanged += OnExpChanged;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.OnExpChanged -= OnExpChanged;
            }
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnExpChanged(float exp)
        {
            UpdateLevel();
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetModifier(stat))
                * (1 + GetPercentageModifier(stat) / 100);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public int UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value && currentLevel.value > 0)
            {
                currentLevel.value = newLevel;
                OnLevelChanged?.Invoke(currentLevel.value);
                LevelUpEffect();
            }
            return currentLevel.value;
        }

        private int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();
            if (exp == null) return startingLevel;

            float currentEXP = exp.ExpPoints;
            int maxLevel = progression.GetLevels(Stat.EXPToLevelUp, characterClass);

            for (int level = 1; level <= maxLevel - 1; level++)
            {
                float EXPToLevelUp = progression.GetStat(characterClass, Stat.EXPToLevelUp, level);

                if (EXPToLevelUp > currentEXP)
                {
                    return level;
                }
            }
            return maxLevel;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        private float GetModifier(Stat stat)
        {
            if (!useModifiers) return 0f;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!useModifiers) return 0f;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private void LevelUpEffect()
        {
            if (levelUpEffect == null) return;

            levelUpEffect.gameObject.SetActive(true);
            levelUpEffect.Play();
        }
    }
}