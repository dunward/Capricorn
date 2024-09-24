using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class TextDisplayer : ActionPlayer
    {
        private bool skip = false;

        public TextDisplayer(NodeActionData actionData) : base(actionData)
        {
        }

        public override IEnumerator Run()
        {
            Debug.LogError($"{actionData.scripts[0]}");
            yield return null;
        }

        public override int Next()
        {
            return actionData.connections[0];
        }
    }
}