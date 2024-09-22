#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Dunward
{
    public class CapricornGraphCoroutineSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var emptyTexture = new Texture2D(1, 1);
            emptyTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
            emptyTexture.Apply();

            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Add Coroutines")),
                new SearchTreeEntry(new GUIContent("Coroutine 1", emptyTexture))
                {
                    level = 1,
                },
                new SearchTreeEntry(new GUIContent("Coroutine 1"))
                {
                    level = 1,
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