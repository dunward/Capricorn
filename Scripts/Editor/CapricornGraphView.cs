using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Newtonsoft.Json;

namespace Dunward
{
    public class CapricornGraphView : GraphView
    {
        private int lastNodeID = 0;

        public CapricornGraphView()
        {
            var node = new CapricornGraphNode(this, -1, new Vector2(100, 200));
            AddElement(node);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new ContextualMenuManipulator(evt => evt.menu.AppendAction("Add Node", action => AddNode(action.eventInfo.localMousePosition), DropdownMenuAction.AlwaysEnabled)));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        
        public Vector2 GetCurrentMousePosition()
        {
            // Get Screen mouse position]
            return this.contentViewContainer.LocalToWorld(Event.current.mousePosition);
        }

        public string SerializeGraph()
        {
            var data = new CapricornGraphData();
            foreach (var node in nodes)
            {
                if (node is CapricornGraphNode capricornGraphNode)
                {
                    data.nodes.Add(capricornGraphNode.GetMainData());
                }
            }
            return JsonConvert.SerializeObject(data);
        }

        private void AddNode(Vector2 position)
        {
            var node = new CapricornGraphNode(this, lastNodeID, position);
            AddElement(node);
            lastNodeID++;
        }
    }
}