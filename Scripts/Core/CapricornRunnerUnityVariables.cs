using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    public partial class CapricornRunner
    {
        internal object nameTarget;
        internal object subNameTarget;
        internal object scriptTarget;

        internal Transform characterArea;
        internal Transform backgroundArea;
        internal Transform foregroundArea;

        private Ref<GameObject> lastBackground = new Ref<GameObject>(null);
        private Ref<GameObject> lastForeground = new Ref<GameObject>(null);
        private Ref<GameObject> bgmObject = new Ref<GameObject>(null);
        private Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();
    }
}