#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public class OutputNode : BaseNode
    {
        public OutputNode() : base()
        {
            nodeType = NodeType.Output;
        }

        public OutputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
        }

        public OutputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
        }

        public OutputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
        }
        
        protected override void SetupTitleContainer()
        {
            var topHeader = new VisualElement();
            topHeader.AddToClassList("capricorn-title-container-output");
            titleContainer.Add(topHeader);
            title = "Output";
        }
    }
}
#endif