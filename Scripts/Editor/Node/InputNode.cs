#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Codice.CM.Interfaces;

namespace Dunward.Capricorn
{
    public class InputNode : BaseNode
    {
        public InputNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            Initialize();
        }

        public InputNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
            Initialize();
        }

        public InputNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            nodeType = NodeType.Input;
            capabilities &= ~Capabilities.Deletable;
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