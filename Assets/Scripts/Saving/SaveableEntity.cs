using UnityEngine;

namespace RPG.Saving
{
    public class SaveableEntity : MonoBehaviour
    {
        public string GetID()
        {
            return "";
        }

        public object CaptureState()
        {
            Debug.Log("Capturing state for " + GetID());
            return null;
        }

        public void RestoreState(object state)
        {
            Debug.Log("Restoring state for " + GetID());
        }
    }
}

