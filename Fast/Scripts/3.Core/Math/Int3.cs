using System;

namespace Fast
{
    [Serializable]
    public class Int3
    {
        private int x = 0;
        private int y = 0;
        private int z = 0;

        public Int3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Int3(Float3 float3)
        {
            this.x = (int)float3.X;
            this.y = (int)float3.Y;
            this.z = (int)float3.Z;
        }

        public Int3(UnityEngine.Vector3Int vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public static bool operator ==(Int3 v1, Int3 v2)
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

        public static bool operator !=(Int3 v1, Int3 v2)
        {
            return !(v1 == v2);
        }

        public static Int3 operator +(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Int3 operator -(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
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
            return this == (obj as Int3);
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public UnityEngine.Vector3Int ToVector3Int()
        {
            return new UnityEngine.Vector3Int(x, y, z);
        }

        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3(x, y, z);
        }
    }
}
