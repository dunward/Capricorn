using System.Runtime.Remoting.Contexts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

        public NodeMainData GetMainData()
        {
            main.action.SerializeConnections();

            var mainData = new NodeMainData();
            mainData.id = id;
            mainData.x = GetPosition().x;
            mainData.y = GetPosition().y;
            mainData.actionData = main.action.data;

            return mainData;
        }

        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.action.DeserializeConnections(mainData.actionData);
        }
    }
}