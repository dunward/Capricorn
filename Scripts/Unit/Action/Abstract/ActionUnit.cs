using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public abstract class ActionUnit
    {
        private int _selectionCount = 1;
        public int SelectionCount
        {
            get => _selectionCount;
            set
            {
                _selectionCount = value;
                OnSelectionCountChanged?.Invoke();
            }
        }
        public event System.Action OnSelectionCountChanged;

        public List<int> connections;

#if UNITY_EDITOR
        public abstract void OnGUI();

        public virtual void InitializeOnCreate()
        {
            
        }
#endif

        protected bool isComplete = false;
        protected int nextConnection = 0;

        public abstract IEnumerator Execute(params object[] args);

        public virtual int GetNextNodeIndex()
        {
            return connections[nextConnection];
        }
    }
}