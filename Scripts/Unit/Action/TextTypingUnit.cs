using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class TextTypingUnit : ActionUnit
    {
        public string name;
        public string subName;
        public string script;

        private bool skip = false;
        private bool isFinish = false;

#if UNITY_EDITOR
        public override void OnGUI()
        {
            GUIStyle style = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true
            };

            name = EditorGUILayout.TextField("Name", name);
            subName = EditorGUILayout.TextField("Sub Name", subName);
            EditorGUILayout.LabelField("Script");
            script = EditorGUILayout.TextArea(script, style, GUILayout.MaxHeight(50));
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            args[0].SetText(name);
            args[1].SetText(subName);
            args[2].SetText(string.Empty);

            foreach (var letter in script)
            {
                if (skip)
                {
                    args[2].SetText(script);
                    break;
                }

                args[2].AppendText(letter);
                yield return new WaitForSeconds(0.05f);
            }

            isFinish = true;

            yield return new WaitUntil(() => isComplete);
        }

        public void Interaction()
        {
            if (isFinish) isComplete = true;
            
            skip = true;
        }
    }
}