#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;

namespace Dunward
{
    public class CapricornGraphNodeCoroutineContainer
    {
        private bool foldout = false;

        public CapricornGraphNodeCoroutineContainer(CapricornGraphNodeMainContainer main)
        {
            
            var test = new List<CapricornGraphCoroutineElement>();
            var list = new ReorderableList(test, typeof(string), true, false, true, true);

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = test[index];
                element.OnGUI(rect, index, isActive, isFocused);
            };

            list.elementHeightCallback = (int index) =>
            {
                var element = test[index];
                return element.GetHeight();
            };

            list.onAddCallback = (ReorderableList l) =>
            {
                test.Add(new CapricornGraphTest());
            };
            
            main.coroutineContainer.Add(new IMGUIContainer(() =>
            {
                foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Coroutine List");
                if (foldout)
                {
                    list.DoLayoutList();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }));
        }
    }
}
#endif