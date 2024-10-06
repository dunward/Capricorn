using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public abstract class FadeUnit : CoroutineUnit
    {
        public bool fade;
        public float elapsedTime;
        public AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

#if UNITY_EDITOR
        protected override string info => "Coroutine Element";

        public override void OnGUI(Rect rect, ref float height)
        {
            fade = UnityEditor.EditorGUI.Toggle(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Fade", fade);
            height += UnityEditor.EditorGUIUtility.singleLineHeight;

            if (fade)
            {
                elapsedTime = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Elapsed Time", elapsedTime),
                    0, float.MaxValue);
                height += UnityEditor.EditorGUIUtility.singleLineHeight;

                lerpCurve = UnityEditor.EditorGUI.CurveField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Lerp Curve", lerpCurve);
                height += UnityEditor.EditorGUIUtility.singleLineHeight;
            }
        }

        public override float GetHeight()
        {
            return fade ? UnityEditor.EditorGUIUtility.singleLineHeight * 5 : UnityEditor.EditorGUIUtility.singleLineHeight * 3;
        }
#endif
    }
}