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

        private Ref<GameObject> lastBackground = new Ref<GameObject>(null);
    }
}