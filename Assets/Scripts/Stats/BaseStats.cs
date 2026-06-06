using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;


        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, startingLevel);
        }

        public int GetLevel()
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
                    return level;
                }
            }
            return maxLevel;
        }
    }
}