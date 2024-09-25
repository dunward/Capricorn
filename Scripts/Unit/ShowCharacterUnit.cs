#if UNITY_EDITOR
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ShowCharacterUnit : FadeUnit
    {
        protected override string info => "Show Character";

        public override void OnGUI(Rect rect, float height)
        {
            base.OnGUI(rect, height);
        }
    }
}
#endif