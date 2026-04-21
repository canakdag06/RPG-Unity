using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        void Update()
        {
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                Save();
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                Load();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
