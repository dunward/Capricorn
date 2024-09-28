using System.Collections;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ClearDialogueTextUnit : CoroutineUnit
    {
#if UNITY_EDITOR
        protected override string info => $"Clear Dialogue Text";
        protected override bool supportWaitingFinish => false;
#endif

        public override IEnumerator Execute(params object[] args)
        {
            args[0].SetText("");
            args[1].SetText("");
            args[2].SetText("");
            
            yield return null;
        }
    }
}