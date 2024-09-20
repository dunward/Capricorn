
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

            new CapricornGraphNodeMainContainer(this);

            RefreshExpandedState();
        }

        public CapricornGraphNode(int id, Vector2 mousePosition) : this(id, mousePosition.x, mousePosition.y)
        {

        }
    }
}