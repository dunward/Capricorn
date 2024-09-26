using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class WaitUnit : CoroutineUnit
    {
        public float time;

#if UNITY_EDITOR
        protected override string info => $"Wait {time} seconds";

        public override void OnGUI(Rect rect, float height)
        {
            var singleHeight = UnityEditor.EditorGUIUtility.singleLineHeight;
            time = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, singleHeight), "Time", time), 0, float.MaxValue);
        }

        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            yield return new WaitForSeconds(time);
        }
    }
}