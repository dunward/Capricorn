
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dunward
{
    public class CapricornGraphNode : Node
    {
        private EnumField enumField;
        public string guid;

        public CapricornGraphNode(int id, float x, float y)
        {
            mainContainer.Insert(1, extensionContainer);
            title = $"{id}";
            SetPosition(new Rect(x, y, 0, 0));

            var container = new CapricornGraphNodeContainer(this);
            extensionContainer.Add(container.Build());

            var input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            var output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            input.portName = string.Empty;
            output.portName = string.Empty;
            input.portColor = new Color(0.69f, 0.98f, 0.34f);
            output.portColor = new Color(0.69f, 0.98f, 0.34f);

            inputContainer.Add(input);
            outputContainer.Add(output);

            RefreshExpandedState();
        }

        public CapricornGraphNode(int id, Vector2 mousePosition) : this(id, mousePosition.x, mousePosition.y)
        {

        }
    }
}