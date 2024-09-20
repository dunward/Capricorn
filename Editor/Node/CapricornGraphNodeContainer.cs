#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dunward
{
    public class CapricornGraphNodeContainer
    {
        private Node parent;
        private VisualElement mainContainer;

        private bool foldout = false;

        public CapricornGraphNodeContainer(Node node)
        {
            parent = node;

            mainContainer = new VisualElement();
            mainContainer.AddToClassList("capricorn-main-container");

            var coroutineContainer = new VisualElement();
            var actionContainer = new VisualElement();

            coroutineContainer.AddToClassList("capricorn-coroutine-container");
            actionContainer.AddToClassList("capricorn-action-container");
            
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
            
            coroutineContainer.Add(new IMGUIContainer(() =>
            {
                foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Coroutine List");
                if (foldout)
                {
                    list.DoLayoutList();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }));


            var enumField = new EnumField(ActionNodeType.NONE);
            actionContainer.Add(enumField);

            mainContainer.Add(coroutineContainer);
            mainContainer.Add(actionContainer);
        }

        public VisualElement Build()
        {
            return mainContainer;
        }
    }
}
#endif