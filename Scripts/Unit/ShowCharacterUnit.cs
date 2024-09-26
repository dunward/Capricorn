using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ShowCharacterUnit : FadeUnit
    {
#if UNITY_EDITOR
        protected override string info => "Show Character";

        public override void OnGUI(Rect rect, float height)
        {
            base.OnGUI(rect, height);
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var obj = args[0] as GameObject;
            Debug.LogError(obj.name);
            yield return new WaitForSeconds(elapsedTime);
        }
    }
}