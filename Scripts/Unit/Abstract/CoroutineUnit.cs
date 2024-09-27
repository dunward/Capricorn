using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public abstract class CoroutineUnit
    {
        public bool isWaitingUntilFinish;

#if UNITY_EDITOR
        protected virtual string info => "Coroutine Element";
        protected virtual bool supportWaitingFinish => true;

        public virtual void OnGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            var singleHeight = UnityEditor.EditorGUIUtility.singleLineHeight;
            var height = 0f;
            UnityEditor.EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, singleHeight), info, UnityEditor.EditorStyles.boldLabel);
            height += singleHeight;
            if (supportWaitingFinish)
            {
                isWaitingUntilFinish = UnityEditor.EditorGUI.Toggle(new Rect(rect.x, rect.y + height, rect.width, singleHeight), "Waiting Until Finish", isWaitingUntilFinish);
                height += singleHeight;
            }
            OnGUI(rect, ref height);
        }

        public virtual void OnGUI(Rect rect, ref float height)
        {
        }

        public virtual float GetHeight()
        {
            return UnityEditor.EditorGUIUtility.singleLineHeight * (supportWaitingFinish ? 2 : 1);
        }
#endif

        public abstract IEnumerator Execute(params object[] args);
    }
}