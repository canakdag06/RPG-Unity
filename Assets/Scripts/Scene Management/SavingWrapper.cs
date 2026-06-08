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

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }


        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindAnyObjectByType<Fader>();

            fader.FadeOutImmediate();
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


#if UNITY_EDITOR
        [UnityEditor.MenuItem("RPG/Delete Save File (In Play Mode)")]
        static void DeleteSaveFileFromMenu()
        {
            SavingWrapper wrapper = FindAnyObjectByType<SavingWrapper>();
            if (wrapper == null) { Debug.LogWarning("SavingWrapper not found in scene."); return; }
            wrapper.Delete();
        }
#endif

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
