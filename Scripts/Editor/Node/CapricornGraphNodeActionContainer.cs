#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using Newtonsoft.Json;
using System;

namespace Dunward
{
    public class CapricornGraphNodeActionContainer
    {
        public CapricornGraphNodeActionData data = new CapricornGraphNodeActionData();

        private CapricornGraphNodeMainContainer main;
        private Color portColor = new Color(0.69f, 0.98f, 0.34f);

        public CapricornGraphNodeActionContainer(CapricornGraphNodeMainContainer main)
        {
            this.main = main;
            data.onUpdateSelectionCount += UpdateOutputPort;

            main.actionContainer.Add(new IMGUIContainer(OnGUI));

            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = string.Empty;
            inputPort.portColor = portColor;
            main.inputContainer.Add(inputPort);
            
            UpdateOutputPort();
        }

        public void SerializeConnections()
        {
            data.connections = new List<int>();

            for (int i = 0; i < main.outputContainer.childCount; i++)
            {
                var port = main.outputContainer[i] as Port;

                foreach (var connection in port.connections)
                {
                    var inputPort = connection.input;
                    var node = inputPort.node as CapricornGraphNode;
                    data.connections.Add(int.Parse(node.title));
                }
            }
        }

        public void DeserializeConnections(CapricornGraphNodeActionData data)
        {
            this.data = data;
            data.onUpdateSelectionCount += UpdateOutputPort;
            UpdateOutputPort();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            data.actionNodeType = (ActionNodeType)EditorGUILayout.EnumPopup(data.actionNodeType);

            if (data.actionNodeType == ActionNodeType.USER_SCRIPT)
            {
                var temp = EditorGUILayout.IntField(data.SelectionCount, GUILayout.Width(20));
                data.SelectionCount = Mathf.Clamp(temp, 1, 4);
            }
            else
            {
                data.SelectionCount = 1;
            }

            EditorGUILayout.EndHorizontal();

            if (data.actionNodeType != ActionNodeType.NONE)
            {
                data.foldout = EditorGUILayout.BeginFoldoutHeaderGroup(data.foldout, "Details");
                if (data.foldout)
                {
                    if (data.actionNodeType == ActionNodeType.CHARACTER_SCRIPT)
                    {
                        DrawCharacterScript();
                    }
                    else if (data.actionNodeType == ActionNodeType.USER_SCRIPT)
                    {
                        DrawUserScript();
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        private void DrawCharacterScript()
        {
            EditorGUILayout.LabelField("Name");
            data.name = EditorGUILayout.TextField(data.name);
            EditorGUILayout.LabelField("Team");
            data.team = EditorGUILayout.TextField(data.team);
            EditorGUILayout.LabelField("Script");
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
            data.scripts[0] = EditorGUILayout.TextArea(data.scripts[0], textAreaStyle, GUILayout.Height(50));
        }

        private void DrawUserScript()
        {
            data.name = string.Empty;
            data.team = string.Empty;
            for (int i = 0; i < data.SelectionCount; i++)
            {
                EditorGUILayout.LabelField($"{i}");
                data.scripts[i] = EditorGUILayout.TextArea(data.scripts[i], GUILayout.Height(20));
            }
        }

        private void UpdateOutputPort()
        {
            if (main.outputContainer.childCount > data.SelectionCount)
            {
                for (int i = main.outputContainer.childCount - 1; i >= data.SelectionCount; i--)
                {
                    var port = main.outputContainer[i] as Port;
                    main.graphView.DeleteElements(port.connections);
                    main.outputContainer.RemoveAt(i);
                    data.scripts.RemoveAt(i);
                }
            }
            else if (main.outputContainer.childCount < data.SelectionCount)
            {
                for (int i = main.outputContainer.childCount; i < data.SelectionCount; i++)
                {
                    var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                    outputPort.portName = $"{i}";
                    outputPort.portColor = portColor;
                    main.outputContainer.Add(outputPort);
                    data.scripts.Add(string.Empty);
                }
            }
        }
    }
}
#endif