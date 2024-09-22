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
                var menu = ScriptableObject.CreateInstance<CapricornGraphCoroutineSearchWindow>();
                Debug.LogError($"{main.GetCurrentMousePosition}");
                SearchWindow.Open(new SearchWindowContext(main.GetCurrentMousePosition), menu);
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

    internal class CapricornGraphCoroutineSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Coroutine"), 1),
                new SearchTreeEntry(new GUIContent("Coroutine 1"))
                {
                    level = 1
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