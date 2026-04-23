using RPG.Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        private float fadeInTime = 1f;

        private IEnumerator Start()
        {
            Fader fader = FindAnyObjectByType<Fader>();

            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

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
