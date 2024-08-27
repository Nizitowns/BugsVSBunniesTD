using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class DirectionData
    {
        public eDirection eDirection;

        public Quaternion GetRotation
        {
            get
            {
                Vector3 vector = new Vector3(0,0,0);
                switch (eDirection)
                {
                    case eDirection.Forward:
                        vector = new Vector3(0, 0, 0);
                        break;
                    case eDirection.Back:
                        vector = new Vector3(0, -180, 0);
                        break;
                    case eDirection.Right:
                        vector = new Vector3(0, 90, 0);
                        break;
                    case eDirection.Left:
                        vector = new Vector3(0, -90, 0);
                        break;
                }
                
                Quaternion direction = Quaternion.identity;
                direction.eulerAngles = vector;

                return direction;
            }
        }
    }

    public enum eDirection
    {
        Forward,
        Back,
        Right,
        Left,
    }
}