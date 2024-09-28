using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Dunward.Capricorn
{
    public class SelectionDisplayer : ActionPlayer
    {
        public SelectionDisplayer(NodeActionData actionData) : base(actionData)
        {
        }

        public List<string> GetSelections()
        {
            return actionData.scripts;
        }

        public IEnumerator Execute(List<Button> buttons, float selectionDestroyAfterDelay)
        {
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