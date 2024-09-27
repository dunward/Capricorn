using System.Collections.Generic;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class GraphData
    {
        public CapricornVector2 position;
        public float zoomFactor;
        public List<NodeMainData> nodes = new List<NodeMainData>();
    }
}