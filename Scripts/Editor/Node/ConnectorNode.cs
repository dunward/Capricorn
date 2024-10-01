#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dunward.Capricorn
{
    public class ConnectorNode : BaseNode
    {
        private TextField customField;
        private ContextualMenuManipulator convertToOutputMenuManipulator;
        private ContextualMenuManipulator convertToConnectorMenuManipulator;

        public ConnectorNode(GraphView graphView, int id, float x, float y) : base(graphView, id, x, y)
        {
            Initialize();
        }

        public ConnectorNode(GraphView graphView, int id, Vector2 mousePosition) : base(graphView, id, mousePosition)
        {
            Initialize();
        }

        public ConnectorNode(GraphView graphView, NodeMainData mainData) : base(graphView, mainData)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            nodeType = NodeType.Connector;

            convertToOutputMenuManipulator = new ContextualMenuManipulator(evt => evt.menu.AppendAction("Convert to Output Node",
                        (action) => ConvertToOutputNode(),
                        DropdownMenuAction.AlwaysEnabled));

            convertToConnectorMenuManipulator = new ContextualMenuManipulator(evt => evt.menu.AppendAction("Convert to Connector Node",
                        (action) => ConvertToConnectorNode(),
                        DropdownMenuAction.AlwaysEnabled));

            this.AddManipulator(convertToOutputMenuManipulator);
        }

        protected override void SetupTitleContainer()
        {
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

            titleContainer.Insert(0, customField);
        }

        protected override void Repaint()
        {
            customField.value = string.IsNullOrEmpty(customTitle) ? $"{id}" : customTitle;
        }

        private void ConvertToOutputNode()
        {
            nodeType = NodeType.Output;
            var topHeader = new VisualElement();
            topHeader.AddToClassList("capricorn-title-container-output");
            titleContainer.Add(topHeader);
            title = "Output";
            customTitle = string.Empty;

            titleContainer.Remove(customField);
            this.AddManipulator(convertToConnectorMenuManipulator);
            this.RemoveManipulator(convertToOutputMenuManipulator);
        }

        private void ConvertToConnectorNode()
        {
            nodeType = NodeType.Connector;
            SetupTitleContainer();

            titleContainer.RemoveAt(titleContainer.childCount - 1);
            title = string.Empty;

            this.AddManipulator(convertToOutputMenuManipulator);
            this.RemoveManipulator(convertToConnectorMenuManipulator);
        }
    }
}
#endif