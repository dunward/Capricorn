using System.Collections;

using UnityEngine;

using TMPro;

namespace Dunward.Capricorn
{
    public class TextDisplayer : ActionPlayer
    {
        private bool skip = false;
        private bool isFinish = false;

        public TextDisplayer(NodeActionData actionData) : base(actionData)
        {
        }

        public IEnumerator Execute(object name, object subName, object script)
        {
            name.SetText(actionData.name);
            subName.SetText(actionData.subName);
            script.SetText(string.Empty);

            foreach (var letter in actionData.scripts[0])
            {
                if (skip)
                {
                    script.SetText(actionData.scripts[0]);
                    break;
                }

                script.AppendText(letter);
                yield return new WaitForSeconds(0.1f);
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