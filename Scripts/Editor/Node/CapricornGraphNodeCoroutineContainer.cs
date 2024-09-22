#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Experimental.GraphView;

namespace Dunward
{
    public class CapricornGraphNodeCoroutineContainer
    {
        private bool foldout = true;

        public CapricornGraphNodeCoroutineContainer(CapricornGraphNodeMainContainer main)
        {
            var elements = new List<CapricornGraphCoroutineElement>();
            var coroutineList = new ReorderableList(elements, typeof(string), true, false, true, true);
            var container = new IMGUIContainer(() =>
            {
                foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Coroutine List");
                if (foldout)
                {
                    coroutineList.DoLayoutList();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            });

            coroutineList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = elements[index];
                element.OnGUI(rect, index, isActive, isFocused);
            };

            coroutineList.elementHeightCallback = (int index) =>
            {
                var element = elements[index];
                return element.GetHeight();
            };

            coroutineList.onAddCallback = (ReorderableList l) =>
            {
                var menu = ScriptableObject.CreateInstance<CapricornGraphCoroutineSearchWindow>();
                var current = Event.current.mousePosition;
                current.y += 130; // Unity default search window height 320 and header height 30. So, 320 / 2 - 30 = 130
                SearchWindow.Open(new SearchWindowContext(container.ChangeCoordinatesTo(main.graphView, current)), menu);
            };
            
            main.coroutineContainer.Add(container);
        }
    }

    internal class CapricornGraphCoroutineSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Coroutine"), 0),
                new SearchTreeEntry(new GUIContent("Coroutine 1"))
                {
                    level = 0
                },
                new SearchTreeEntry(new GUIContent("Coroutine 2"))
                {
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Coroutine 3"))
                {
                    level = 1
                },
            };
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Debug.Log("Selected");

            return false;
        }
    }
}
#endif