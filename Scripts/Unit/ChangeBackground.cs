using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ChangeBackground : FadeUnit
    {
#if UNITY_EDITOR
        protected override string info => "Change Background";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            yield return new WaitForSeconds(elapsedTime);
        }
    }
}