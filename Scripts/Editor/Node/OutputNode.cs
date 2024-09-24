using System.Runtime.Remoting.Contexts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public class OutputNode : Node
    {
        public OutputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
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