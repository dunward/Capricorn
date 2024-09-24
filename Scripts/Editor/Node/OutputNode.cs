#if UNITY_EDITOR
using UnityEngine;

namespace Dunward.Capricorn
{
    public class OutputNode : BaseNode
    {
        public OutputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            nodeType = NodeType.Output;
        }

        public OutputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
        }

        public OutputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
        }

        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.action.DeserializeConnections(mainData.actionData);
        }

        protected override void SetupTitleContainer()
        {
            titleContainer.style.backgroundColor = CapricornColors.OutputTitleHeader;
            title = "Output";
        }
    }
}
#endif