#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public class OutputNode : BaseNode
    {
        private TextField customField;
        private ContextualMenuManipulator convertToOutputMenuManipulator;
        private ContextualMenuManipulator convertToConnectorMenuManipulator;

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

            
            convertToOutputMenuManipulator = new ContextualMenuManipulator(evt => evt.menu.AppendAction("Convert to Output Node",
                        (action) => ConvertToOutputNode(),
                        DropdownMenuAction.AlwaysEnabled));

            convertToConnectorMenuManipulator = new ContextualMenuManipulator(evt => evt.menu.AppendAction("Convert to Connector Node",
                        (action) => ConvertToConnectorNode(),
                        DropdownMenuAction.AlwaysEnabled));

            this.AddManipulator(convertToConnectorMenuManipulator);
        }
        
        protected override void SetupTitleContainer()
        {
            var topHeader = new VisualElement();
            topHeader.AddToClassList("capricorn-title-container-output");
            titleContainer.Add(topHeader);
            title = "Output";
        }

        private void ConvertToOutputNode()
        {
            nodeType = NodeType.Output;
            SetupTitleContainer();
            customTitle = string.Empty;

            titleContainer.Remove(customField);
            this.AddManipulator(convertToConnectorMenuManipulator);
            this.RemoveManipulator(convertToOutputMenuManipulator);
        }

        private void ConvertToConnectorNode()
        {
            nodeType = NodeType.Connector;
            customField = new TextField() { value = string.IsNullOrEmpty(customTitle) ? $"{id}" : customTitle };
            customField.RegisterValueChangedCallback(evt =>
            {
                customTitle = evt.newValue;
            });
            customField.RegisterCallback<FocusOutEvent>(evt =>
            {
                if (string.IsNullOrEmpty(customField.value))
                {
                    customTitle = string.Empty;
                    customField.value = $"{id}";
                }
            });

            titleContainer.RemoveAt(titleContainer.childCount - 1);
            title = string.Empty;

            titleContainer.Insert(0, customField);

            this.AddManipulator(convertToOutputMenuManipulator);
            this.RemoveManipulator(convertToConnectorMenuManipulator);
        }
    }
}
#endif