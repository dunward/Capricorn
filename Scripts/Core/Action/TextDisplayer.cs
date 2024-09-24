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

        public IEnumerator Execute<T>(T name, T subName, T script) where T : TMP_Text
        {
            name.text = actionData.name;
            subName.text = actionData.subName;
            script.text = string.Empty;

            foreach (var letter in actionData.scripts[0])
            {
                if (skip)
                {
                    script.text = actionData.scripts[0];
                    break;
                }

                script.text += letter;
                yield return new WaitForSeconds(0.1f);
            }

            isFinish = true;

            yield return new WaitUntil(() => isComplete);
        }

        public override int Next()
        {
            return actionData.connections[0];
        }

        public void Interaction()
        {
            if (isFinish) isComplete = true;
            
            skip = true;
        }
    }
}