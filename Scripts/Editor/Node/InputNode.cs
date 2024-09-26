#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dunward.Capricorn
{
    public class InputNode : BaseNode
    {
        public InputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            nodeType = NodeType.Input;
            capabilities &= ~Capabilities.Deletable;
        }

        public InputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
        }

        public InputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
        }

        protected override void SetupTitleContainer()
        {
            var topHeader = new VisualElement();
            topHeader.AddToClassList("capricorn-title-container-input");
            titleContainer.Add(topHeader);
            title = "Input";
        }
    }
}
#endif