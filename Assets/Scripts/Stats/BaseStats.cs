using System;
using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        [SerializeField] ParticleSystem levelUpEffect;

        public event Action<int> OnLevelChanged;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();

            Experience exp = GetComponent<Experience>();
            if (exp != null) exp.OnExpChanged += OnExpChanged;
        }

        private void OnDestroy()
        {
            Experience exp = GetComponent<Experience>();
            if (exp != null) exp.OnExpChanged -= OnExpChanged;
        }

        private void OnExpChanged(float exp)
        {
            UpdateLevel();
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel()) + GetModifier(stat);
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        public int UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                OnLevelChanged?.Invoke(currentLevel);
                LevelUpEffect();
            }
            return currentLevel;
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

        private float GetModifier(Stat stat)
        {
            float total = 0f;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetModifier(stat))
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