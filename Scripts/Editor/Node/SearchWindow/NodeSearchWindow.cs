#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;

namespace Dunward.Capricorn
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public Action<Type, Vector2> onSelectNode;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var icon = EditorGUIUtility.FindTexture("d_UnityEditor.Graphs.AnimatorControllerTool");

            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Add Nodes")),
                new SearchTreeEntry(new GUIContent("Connector", icon))
                {
                    level = 1,
                    userData = typeof(ConnectorNode),
                },
                new SearchTreeEntry(new GUIContent("Output", icon))
                {
                    level = 1,
                    userData = typeof(OutputNode),
                },
            };
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var position = context.screenMousePosition;
            onSelectNode?.Invoke(SearchTreeEntry.userData as Type, position);
            return true;
        }
    }
}
#endif