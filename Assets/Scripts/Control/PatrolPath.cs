using UnityEngine;


namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWayPoint(i), 0.25f);
                int j = (i + 1) % transform.childCount;

                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        private Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
