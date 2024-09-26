#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public class OutputNode : BaseNode
    {
        public OutputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            Initialize();
        }

        public OutputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
            Initialize();
        }

        public OutputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            nodeType = NodeType.Output;
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