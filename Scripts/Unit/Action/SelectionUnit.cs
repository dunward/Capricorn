using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class SelectionUnit : ActionUnit
    {
        public List<string> scripts = new List<string>();

#if UNITY_EDITOR
        public override void OnGUI()
        {
            var newCount = EditorGUILayout.IntSlider("Selection Count", SelectionCount, 1, 4);
            
            if (newCount != SelectionCount)
            {
                SelectionCount = newCount;
                
                if (scripts.Count < SelectionCount)
                {
                    scripts.AddRange(Enumerable.Repeat(string.Empty, SelectionCount - scripts.Count));
                }
                else if (scripts.Count > SelectionCount)
                {
                    scripts = scripts.Take(SelectionCount).ToList();
                }
            }

            for (int i = 0; i < SelectionCount; i++)
            {
                EditorGUILayout.LabelField($"Selection {i + 1}");
                scripts[i] = EditorGUILayout.TextArea(scripts[i]);
            }

            scripts = scripts.Take(SelectionCount).ToList();
        }

        public override void InitializeOnCreate()
        {
            scripts.Add(string.Empty);
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            isComplete = false;
            
            var buttons = args[0] as List<Button>;
            var selectionDestroyAfterDelay = (float)args[1];

            for (int i = 0; i < buttons.Count; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() =>
                {
                    isComplete = true;
                    nextConnection = index;
                });
            }

            yield return new WaitUntil(() => isComplete);
            
            yield return new WaitForSeconds(selectionDestroyAfterDelay);
        }
    }
}