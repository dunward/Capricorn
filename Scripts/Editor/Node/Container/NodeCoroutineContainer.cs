#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Experimental.GraphView;
using System;

namespace Dunward.Capricorn
{
    public class NodeCoroutineContainer
    {
        private ReorderableList coroutineList;

        private List<CoroutineUnit> coroutines = new List<CoroutineUnit>();
        private bool foldout = true;

        public NodeCoroutineData CoroutineData
        {
            get
            {
                var data = new NodeCoroutineData();
                data.coroutines = coroutines;
                return data;
            }
        }

        public NodeCoroutineContainer(NodeMainContainer main)
        {
            coroutineList = new ReorderableList(coroutines, typeof(string), true, false, true, true);
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
                var element = coroutines[index];
                element.OnGUI(rect, index, isActive, isFocused);
            };

            coroutineList.elementHeightCallback = (int index) =>
            {
                var element = coroutines[index];
                return element.GetHeight();
            };

            coroutineList.onAddCallback = (ReorderableList l) =>
            {
                var menu = ScriptableObject.CreateInstance<CoroutineSearchWindow>();
                menu.Initialize(l);

                var current = Event.current.mousePosition;
                var calc = container.ChangeCoordinatesTo(main.graphView, current);
                calc.y += 160; // Unity default search window height 320.
                SearchWindow.Open(new SearchWindowContext(calc), menu);
            };
            
            main.coroutineContainer.Add(container);
        }

        public void DeserializeCoroutines(NodeCoroutineData coroutineData)
        {
            if (coroutineData == null) return;

            coroutines = coroutineData.coroutines;
            coroutineList.list = coroutines;
        }
    }
}
#endif