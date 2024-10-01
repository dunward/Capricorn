using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [DisallowMultipleComponent]
    public class CapricornCache : MonoBehaviour
    {
        internal Ref<GameObject> lastBackground = new Ref<GameObject>(null);
        internal Ref<GameObject> lastForeground = new Ref<GameObject>(null);
        internal Ref<GameObject> bgmObject = new Ref<GameObject>(null);
    }
}