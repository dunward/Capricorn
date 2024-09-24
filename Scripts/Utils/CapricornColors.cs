#if UNITY_EDITOR
using UnityEngine;

namespace Dunward.Capricorn
{
    public static class CapricornColors
    {
        public static Color InputTitleHeader => Color.green;
        public static Color OutputTitleHeader => Color.red;
        public static Color Port => new Color(0.69f, 0.98f, 0.34f);
    }
}
#endif