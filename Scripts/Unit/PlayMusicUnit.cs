using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class PlayMusicUnit : FadeUnit
    {
#if UNITY_EDITOR
        protected override string info => "Play Music";

        public override void OnGUI(Rect rect, float height)
        {
            base.OnGUI(rect, height);
        }
#endif
    }
}