using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class NodeActionData
    {
        public ActionUnit action = new NoneActionUnit();
    }
}