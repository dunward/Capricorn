using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class TextDisplayer : ActionPlayer
    {
        private NodeActionData actionData;

        public TextDisplayer(NodeActionData actionData)
        {
            this.actionData = actionData;
        }

        public override IEnumerator Run()
        {
            Debug.LogError($"{actionData.scripts[0]}");
            yield return null;
        }
    }
}