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

        public event Action<int> OnLevelChanged;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, startingLevel);
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();
            if(exp == null) return startingLevel;

            float currentEXP = exp.ExpPoints;
            int maxLevel = progression.GetLevels(Stat.EXPToLevelUp, characterClass);

            for (int level = 1; level <= maxLevel - 1; level++)
            {
                float EXPToLevelUp = progression.GetStat(characterClass, Stat.EXPToLevelUp, level);

                if(EXPToLevelUp > currentEXP)
                {
                    OnLevelChanged?.Invoke(level);
                    return level;
                }
            }
            OnLevelChanged?.Invoke(maxLevel);
            return maxLevel;
        }
    }
}