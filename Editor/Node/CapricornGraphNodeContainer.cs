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
            var test = new List<string>{ "Test1", "Test2", "Test3" };
            var list = new ReorderableList(test, typeof(string), true, false, true, true);
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