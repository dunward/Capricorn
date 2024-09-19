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
            this.AddManipulator(new ContextualMenuManipulator(evt => evt.menu.AppendAction("Add Node", action => AddElement(new CapricornGraphNode(action.eventInfo.localMousePosition)), DropdownMenuAction.AlwaysEnabled)));
        }
    }
}