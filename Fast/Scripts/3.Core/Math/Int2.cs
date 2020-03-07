using System;

namespace Fast
{
    [Serializable]
    public class Int2
    {
        private int x = 0;
        private int y = 0;

        public Int2()
        {
            this.x = 0;
            this.y = 0;
        }

        public Int2(Int2 int2)
        {
            this.x = int2.x;
            this.y = int2.y;
        }

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Int2(Float2 float2)
        {
            this.x = (int)float2.X;
            this.y = (int)float2.Y;
        }

        public Int2(UnityEngine.Vector2Int vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public static bool operator ==(Int2 v1, Int2 v2)
        {
            if (!object.ReferenceEquals(v1, null) && !object.ReferenceEquals(v2, null))
            {
                return v1.X == v2.X && v1.Y == v2.Y;
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

        public static bool operator !=(Int2 v1, Int2 v2)
        {
            return !(v1 == v2);
        }

        public static Int2 operator +(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Int2 operator -(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static bool operator ==(Int2 v1, UnityEngine.Vector2Int v2)
        {
            if (!object.ReferenceEquals(v1, null) && !object.ReferenceEquals(v2, null))
            {
                return v1.X == v2.x && v1.Y == v2.y;
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

        public static bool operator !=(Int2 v1, UnityEngine.Vector2Int v2)
        {
            return !(v1 == v2);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            return this == (obj as Int2);
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

        public UnityEngine.Vector2Int ToVector2Int()
        {
            return new UnityEngine.Vector2Int(x, y);
        }

        public UnityEngine.Vector2 ToVector2()
        {
            return new UnityEngine.Vector2(x, y);
        }
    }
}
