#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Dunward.Capricorn
{
    public class NodeActionContainer
    {
        public NodeActionData data = new NodeActionData();

        private NodeMainContainer main;

        public NodeActionContainer(NodeMainContainer main)
        {
            this.main = main;

            main.actionContainer.Add(new IMGUIContainer(OnGUI));

            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = string.Empty;
            inputPort.portColor = CapricornColors.Port;
            main.inputContainer.Add(inputPort);
            
            UpdateOutputPort();
        }

        public void SerializeConnections()
        {
            data.action.connections = Enumerable.Range(0, main.outputContainer.childCount)
                                .Select(_ => -999)
                                .ToList();

            for (int i = 0; i < main.outputContainer.childCount; i++)
            {
                var port = main.outputContainer[i] as Port;

                foreach (var connection in port.connections)
                {
                    var inputPort = connection.input;
                    var node = inputPort.node as BaseNode;
                    data.action.connections[i] = node.ID;
                }
            }
        }

        public void DeserializeActions(NodeActionData data)
        {
            this.data = data;
            this.data.action.OnSelectionCountChanged += UpdateOutputPort;

            UpdateOutputPort();
        }

        private void OnGUI()
        {
            var assembly = Assembly.GetAssembly(typeof(ActionUnit));
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ActionUnit)))
                .ToList();

            if (EditorGUILayout.DropdownButton(new GUIContent(data.action?.GetType().Name), FocusType.Passive))
            {
                var menu = new GenericMenu();
                foreach (var type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        data.action = (ActionUnit)Activator.CreateInstance(type);
                        data.action.InitializeOnCreate();
                        data.action.OnSelectionCountChanged += UpdateOutputPort;
                    });
                }
                menu.ShowAsContext();
            }

            if (data.action != null)
            {
                data.action.OnGUI();
            }
        }

        private void UpdateOutputPort()
        {
            if (main.outputContainer.childCount > data.action.SelectionCount)
            {
                for (int i = main.outputContainer.childCount - 1; i >= data.action.SelectionCount; i--)
                {
                    var port = main.outputContainer[i] as Port;
                    main.graphView.DeleteElements(port.connections);
                    main.outputContainer.RemoveAt(i);
                }
            }
            else if (main.outputContainer.childCount < data.action.SelectionCount)
            {
                for (int i = main.outputContainer.childCount; i < data.action.SelectionCount; i++)
                {
                    var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                    outputPort.portName = $"{i}";
                    outputPort.portColor = CapricornColors.Port;
                    main.outputContainer.Add(outputPort);
                }
            }
        }
    }
}
#endif