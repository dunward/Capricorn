using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public abstract class FadeUnit : CoroutineUnit
    {
        public bool fade;
        public float elapsedTime;
#if UNITY_EDITOR
        protected override string info => "Coroutine Element";

        public override void OnGUI(Rect rect, float height)
        {
            fade = UnityEditor.EditorGUI.Toggle(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Fade", fade);
            height += UnityEditor.EditorGUIUtility.singleLineHeight;

            if (fade)
            {
                elapsedTime = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Elapsed Time", elapsedTime),
                    0, float.MaxValue);
            }
        }

        public override float GetHeight()
        {
            return fade ? UnityEditor.EditorGUIUtility.singleLineHeight * 4 : UnityEditor.EditorGUIUtility.singleLineHeight * 3;
        }
#endif
    }
}