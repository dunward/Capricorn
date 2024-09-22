#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using System.Linq;

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
                var assembly = Assembly.GetAssembly(typeof(CapricornGraphCoroutineElement));
                var derivedTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CapricornGraphCoroutineElement)))
                    .ToList();

                foreach (var type in derivedTypes)
                {
                    Debug.LogError(type);
                }
                
                var current = Event.current.mousePosition;
                current.y += 130; // Unity default search window height 320 and header height 30. So, 320 / 2 - 30 = 130
                SearchWindow.Open(new SearchWindowContext(container.ChangeCoordinatesTo(main.graphView, current)), menu);
            };
            
            main.coroutineContainer.Add(container);
        }
    }
}
#endif