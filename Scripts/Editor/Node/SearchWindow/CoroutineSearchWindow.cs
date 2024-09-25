#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;

namespace Dunward.Capricorn
{
    public class CoroutineSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private ReorderableList list;

        public void Initialize(ReorderableList list)
        {
            this.list = list;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var emptyTexture = new Texture2D(1, 1);
            emptyTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
            emptyTexture.Apply();

            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Add Coroutines")),
            };

            var assembly = Assembly.GetAssembly(typeof(CoroutineUnit));
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CoroutineUnit)))
                .ToList();

            foreach (var type in types)
            {
                entries.Add(new SearchTreeEntry(new GUIContent(type.Name, emptyTexture))
                {
                    level = 1,
                    userData = type,
                });
            }

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            list.list.Add((CoroutineUnit)System.Activator.CreateInstance(SearchTreeEntry.userData as System.Type));
            return true;
        }
    }
}
#endif