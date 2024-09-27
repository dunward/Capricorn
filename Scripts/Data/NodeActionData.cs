using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class NodeActionData
    {
        public ActionType actionNodeType = ActionType.NONE;

        public bool foldout = true;
        public List<int> connections;

        public string name;
        public string subName;
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