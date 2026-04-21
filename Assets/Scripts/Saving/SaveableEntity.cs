using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string ID = "";

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject sObject = new SerializedObject(this);
            SerializedProperty property = sObject.FindProperty("ID");

            if (property.stringValue == "")
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                sObject.ApplyModifiedProperties();
            }

            print("Editing");
        }
#endif
        public string GetID()
        {
            return ID;
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

