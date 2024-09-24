using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class SelectionDisplayer : ActionPlayer
    {
        public SelectionDisplayer(NodeActionData actionData) : base(actionData)
        {
        }

        public IEnumerator Run()
        {
            Debug.LogError($"{string.Join(", ", actionData.scripts)}");
            yield return null;
        }
    }
}