using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);

            CaptureState(state);

            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);

            if(state.ContainsKey("lastSceneBuildIndex"))
            {
                int index = (int)state["lastSceneBuildIndex"];

                if (index != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(index);
                }
                else
                {
                    Debug.Log("Same scene, no need to load");
                }
            }

            RestoreState(state);

        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return $"{Application.persistentDataPath}/{saveFile}.sav";
            //return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                state[saveable.GetID()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                string id = saveable.GetID();
                if(state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }
    }
}
