using System;

namespace Fast
{
    [Serializable]
    public class Float2
    {
        private float x = 0;
        private float y = 0;

        public Float2()
        {
            this.x = 0.0f;
            this.y = 0.0f;
        }

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Float2(Int2 int2)
        {
            this.x = int2.X;
            this.y = int2.Y;
        }

        public Float2(UnityEngine.Vector2 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public static bool operator ==(Float2 v1, Float2 v2)
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

        public static bool operator !=(Float2 v1, Float2 v2)
        {
            return !(v1 == v2);
        }

        public static Float2 operator +(Float2 v1, Float2 v2)
        {
            return new Float2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Float2 operator -(Float2 v1, Float2 v2)
        {
            return new Float2(v1.X - v2.X, v1.Y - v2.Y);
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
            return this == (obj as Float2);
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

        public UnityEngine.Vector2 ToVector2()
        {
            return new UnityEngine.Vector2(x, y);
        }

        public UnityEngine.Vector2Int ToVector2Int()
        {
            return new UnityEngine.Vector2Int((int)x, (int)y);
        }
    }
}