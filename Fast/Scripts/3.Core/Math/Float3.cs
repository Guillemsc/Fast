using System;

namespace Fast
{
    [Serializable]
    public class Float3
    {
        private float x = 0;
        private float y = 0;
        private float z = 0;

        public Float3()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
        }

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Float3(Int3 int3)
        {
            this.x = int3.X;
            this.y = int3.Y;
            this.z = int3.Z;
        }

        public Float3(UnityEngine.Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public static bool operator ==(Float3 v1, Float3 v2)
        {
            if (!object.ReferenceEquals(v1, null) && !object.ReferenceEquals(v2, null))
            {
                return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
            }
            else if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Float3 v1, Float3 v2)
        {
            return !(v1 == v2);
        }

        public static Float3 operator +(Float3 v1, Float3 v2)
        {
            return new Float3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Float3 operator -(Float3 v1, Float3 v2)
        {
            return new Float3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            hash = hash * 23 + Z.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            return this == (obj as Float3);
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public UnityEngine.Vector3Int ToVector3Int()
        {
            return new UnityEngine.Vector3Int((int)x, (int)y, (int)z);
        }
    }
}
