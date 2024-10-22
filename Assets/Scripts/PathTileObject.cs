using UnityEngine;

namespace DefaultNamespace
{
    [SelectionBase]
    public class PathTileObject : MonoBehaviour
    {
        public bool HasObstacle = false;
        
        public DirectionData directionData = new DirectionData();
        
        private void OnDrawGizmos()
        {
            if (!Debugger.ShowObstacleFacingDirection) return;
            
            Gizmos.color = Color.green;
            Vector3 startPos = transform.position + (transform.up * 3);
            switch (directionData.eDirection)
            {
                case eDirection.Forward:
                    Gizmos.DrawRay(startPos, Vector3.forward * 5);
                    break;
                case eDirection.Back:
                    Gizmos.DrawRay(startPos, Vector3.back * 5);
                    break;
                case eDirection.Right:
                    Gizmos.DrawRay(startPos, Vector3.right * 5);
                    break;
                case eDirection.Left:
                    Gizmos.DrawRay(startPos, Vector3.left * 5);
                    break;
            }
        }
    }
}