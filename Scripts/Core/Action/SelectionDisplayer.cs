using System.Collections;
using System.Linq;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class SelectionDisplayer : ActionPlayer
    {
        public SelectionDisplayer(NodeActionData actionData) : base(actionData)
        {
        }

        public override IEnumerator Run()
        {
            Debug.LogError($"{string.Join(", ", actionData.scripts)}");
            yield return null;
        }
    }
}