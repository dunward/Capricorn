#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

using Newtonsoft.Json;

namespace Dunward.Capricorn
{
    public class NodeActionContainer
    {
        public NodeActionData data = new NodeActionData();

        private NodeMainContainer main;

        public NodeActionContainer(NodeMainContainer main)
        {
            this.main = main;
            data.onUpdateSelectionCount += UpdateOutputPort;

            main.actionContainer.Add(new IMGUIContainer(OnGUI));

            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = string.Empty;
            inputPort.portColor = CapricornColors.Port;
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
                    var node = inputPort.node as Node;
                    data.connections.Add(int.Parse(node.title));
                }
            }
        }

        public void DeserializeConnections(NodeActionData data)
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
            EditorGUILayout.LabelField("Sub Name");
            data.subName = EditorGUILayout.TextField(data.subName);
            EditorGUILayout.LabelField("Script");
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
            data.scripts[0] = EditorGUILayout.TextArea(data.scripts[0], textAreaStyle, GUILayout.Height(50));
        }

        private void DrawUserScript()
        {
            data.name = string.Empty;
            data.subName = string.Empty;
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
                }
            }
            else if (main.outputContainer.childCount < data.SelectionCount)
            {
                for (int i = main.outputContainer.childCount; i < data.SelectionCount; i++)
                {
                    var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                    outputPort.portName = $"{i}";
                    outputPort.portColor = CapricornColors.Port;
                    main.outputContainer.Add(outputPort);
                    data.scripts.Add(string.Empty);
                }
            }

            data.scripts = data.scripts.Take(data.SelectionCount).ToList();
        }
    }
}
#endif