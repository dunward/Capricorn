namespace Dunward.Capricorn
{
    [System.Serializable]
    public struct CapricornVector2
    {
        public float x;
        public float y;

        public static implicit operator CapricornVector2(UnityEngine.Vector2 vector2)
        {
            return new CapricornVector2
            {
                x = vector2.x,
                y = vector2.y
            };
        }

        public static implicit operator UnityEngine.Vector2(CapricornVector2 vector2)
        {
            return new UnityEngine.Vector2(vector2.x, vector2.y);
        }

        public static implicit operator CapricornVector2(UnityEngine.Vector3 vector2)
        {
            return new CapricornVector2
            {
                x = vector2.x,
                y = vector2.y
            };
        }

        public static implicit operator UnityEngine.Vector3(CapricornVector2 vector2)
        {
            return new UnityEngine.Vector3(vector2.x, vector2.y, 0);
        }
    }
}