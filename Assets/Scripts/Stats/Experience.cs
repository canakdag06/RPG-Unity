using RPG.Saving;
using System;
using UnityEngine;


namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;
        public event Action<float> OnExpChanged;

        public float ExpPoints => experiencePoints;

        public void GainEXP(float exp)
        {
            experiencePoints += exp;
            OnExpChanged?.Invoke(experiencePoints);
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
            OnExpChanged?.Invoke(experiencePoints);
        }
    }
}
