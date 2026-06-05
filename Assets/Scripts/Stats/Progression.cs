using RPG.Stats;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionLevel[] levels;
        }

        [System.Serializable]
        public class ProgressionLevel
        {
            public float health;
            public float expReward;
        }

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                if(progressionClass.characterClass == characterClass)
                {
                    return progressionClass.levels[level - 1].health;
                }
            }
            return 0;
        }

        public float GetEXPReward(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                if(progressionClass.characterClass == characterClass)
                {
                    return progressionClass.levels[level - 1].expReward;
                }
            }
            return 0;
        }
    }
}