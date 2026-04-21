using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

