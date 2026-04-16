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
                GetComponent<SavingSystem>().Save(defaultSaveFile);
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                GetComponent<SavingSystem>().Load(defaultSaveFile);
            }
        }
    }
}
