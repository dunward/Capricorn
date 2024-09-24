using System.Collections;
using System.Linq;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class SelectionDisplayer : ActionPlayer
    {
        private NodeActionData actionData;

        public SelectionDisplayer(NodeActionData actionData)
        {
            this.actionData = actionData;
        }

        public override IEnumerator Run()
        {
            Debug.LogError($"{string.Join(", ", actionData.scripts)}");
            yield return null;
        }
    }
}