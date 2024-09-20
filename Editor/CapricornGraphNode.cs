
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dunward
{
    public class CapricornGraphNode : Node
    {
        private EnumField enumField;
        public string guid;

        public CapricornGraphNode(float x, float y)
        {
            mainContainer.Insert(1, extensionContainer);
            title = "Node";
            SetPosition(new Rect(x, y, 0, 0));
            Debug.LogError($"Position: {x}, {y}");

            // Middle
            var textField = new TextField();
            textField.value = "TEST VALUE";
            extensionContainer.Add(textField);

            // Bottom
            enumField = new EnumField(ActionNodeType.NONE);
            extensionContainer.Add(enumField);

            var input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            var output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            input.portName = string.Empty;
            output.portName = string.Empty;

            inputContainer.Add(input);
            outputContainer.Add(output);

            RefreshExpandedState();
        }

        public CapricornGraphNode(Vector2 mousePosition) : this(mousePosition.x, mousePosition.y)
        {

        }
    }
}