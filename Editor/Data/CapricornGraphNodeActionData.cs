using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dunward
{
    [System.Serializable]
    public class CapricornGraphNodeActionData
    {
        public ActionNodeType actionNodeType = ActionNodeType.NONE;

        public bool foldout = true;
        public List<string> scripts = new List<string>();

        private int _selectionCount = 1;
        public int SelectionCount
        {
            get => _selectionCount;
            set
            {
                _selectionCount = value;
                onUpdateSelectionCount?.Invoke();
            }
        }

        [JsonIgnore]
        public Action onUpdateSelectionCount;
    }
}