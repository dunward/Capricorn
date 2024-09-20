
using UnityEditor;
using UnityEngine;

namespace Dunward
{
    [System.Serializable]
    public abstract class CapricornGraphCoroutineElement
    {
        protected bool isWaitingUntilFinish;
        protected virtual string info => "Coroutine Element";

        public virtual void OnGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y -= 10;
            EditorGUI.LabelField(rect, info);
            rect.y += EditorGUIUtility.singleLineHeight;
            isWaitingUntilFinish = EditorGUI.ToggleLeft(rect, "Waiting Until Finish", isWaitingUntilFinish);
        }

        public virtual float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}