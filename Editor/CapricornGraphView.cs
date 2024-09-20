using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dunward
{
    public class CapricornGraphView : GraphView
    {
        public CapricornGraphView()
        {
            // var node = new CapricornGraphNode();
            // AddElement(node);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new ContextualMenuManipulator(evt => evt.menu.AppendAction("Add Node", action => AddElement(new CapricornGraphNode(action.eventInfo.localMousePosition)), DropdownMenuAction.AlwaysEnabled)));
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
    }
}