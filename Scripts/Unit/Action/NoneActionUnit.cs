using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class NoneActionUnit : ActionUnit
    {
#if UNITY_EDITOR
        public override void OnGUI()
        {
            
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            yield return null;
        }
    }
}