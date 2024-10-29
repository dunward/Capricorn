using System.Collections;
using System.Collections.Generic;
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
            skip = false;
            isFinish = false;
            isComplete = false;
            
            args[0].SetText(name);
            args[1].SetText(subName);
            args[2].SetText(string.Empty);

            var totalText = new System.Text.StringBuilder();
            var suffix = string.Empty;

            for (int i = 0; i < script.Length; i++)
            {
                if (skip)
                {
                    args[2].SetText(script);
                    break;
                }

                if (script[i] == '<')
                {
                    var tag = string.Empty;
                    bool isClosingTag = false;

                    while (script[i] != '>')
                    {
                        tag += script[i];
                        i++;
                    }
                    tag += '>';

                    if (tag.StartsWith("</"))
                    {
                        isClosingTag = true;
                    }
                    else
                    {
                        var tagName = string.Empty;
                        if (tag.Contains("="))
                        {
                            tagName = tag.Substring(1, tag.IndexOf('=') - 1);
                        }
                        else
                        {
                            tagName = tag.Substring(1, tag.Length - 2);
                        }

                        suffix = $"</{tagName}>{suffix}";
                    }

                    if (isClosingTag)
                    {
                        var closingTagName = tag.Substring(2, tag.Length - 3);
                        if (suffix.Contains($"</{closingTagName}>"))
                        {
                            suffix = suffix.Replace($"</{closingTagName}>", string.Empty);
                        }
                    }

                    totalText.Append(tag);
                }
                else
                {
                    totalText.Append(script[i]);
                    args[2].SetText(string.Concat(totalText, suffix));
                    yield return new WaitForSeconds(0.05f);
                }
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