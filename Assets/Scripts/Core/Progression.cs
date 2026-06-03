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
            [SerializeField] CharacterClass characterClass;
            [SerializeField] ProgressionLevel[] levels;
        }

        [System.Serializable]
        public class ProgressionLevel
        {
            [SerializeField] int level;
            [SerializeField] float health;
            [SerializeField] float damage;
        }

    }
}