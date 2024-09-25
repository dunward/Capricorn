using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public abstract class CoroutineUnit
    {
        public string type;
        public bool isWaitingUntilFinish;

        public CoroutineUnit()
        {
            type = GetType().Name;
        }

#if UNITY_EDITOR
        protected virtual string info => "Coroutine Element";

        public void OnGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            var singleHeight = UnityEditor.EditorGUIUtility.singleLineHeight;
            var height = 0f;
            UnityEditor.EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, singleHeight), info);
            height += singleHeight;
            isWaitingUntilFinish = UnityEditor.EditorGUI.Toggle(new Rect(rect.x, rect.y + height, rect.width, singleHeight), "Waiting Until Finish", isWaitingUntilFinish);
            height += singleHeight;
            OnGUI(rect, height);
        }

        public abstract void OnGUI(Rect rect, float height);

        public virtual float GetHeight()
        {
            return UnityEditor.EditorGUIUtility.singleLineHeight * 2;
        }
#endif
    }
}