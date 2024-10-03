using System.Collections.Generic;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class GraphData
    {
        public CapricornVector2 position;
        public float zoomFactor;
        public int debugNodeIndex;
        public List<NodeMainData> nodes = new List<NodeMainData>();
    }
}