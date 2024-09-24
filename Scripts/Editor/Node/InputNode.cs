using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Dunward.Capricorn
{
    public class InputNode : Node
    {
        public InputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            capabilities &= ~Capabilities.Deletable;
        }

        public InputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
        }

        public InputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
        }

        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.action.DeserializeConnections(mainData.actionData);
        }

        protected override void SetupTitleContainer()
        {
            titleContainer.style.backgroundColor = CapricornColors.InputTitleHeader;
            title = "Input";
        }
    }
}