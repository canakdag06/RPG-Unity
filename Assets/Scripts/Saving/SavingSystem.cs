using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("Loading from " + path);

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return $"{Application.persistentDataPath}/{saveFile}.sav";
            //return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                state[saveable.GetID()] = saveable.CaptureState();
            }

            return state;
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                saveable.RestoreState(stateDict[saveable.GetID()]);
            }
        }
    }
}
