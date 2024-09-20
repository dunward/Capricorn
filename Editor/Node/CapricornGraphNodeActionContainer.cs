#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using Newtonsoft.Json;

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

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            data.actionNodeType = (ActionNodeType)EditorGUILayout.EnumPopup(data.actionNodeType);

            if (data.actionNodeType == ActionNodeType.TEACHER_SPEECH)
            {
                var temp = EditorGUILayout.IntField(data.SelectionCount, GUILayout.Width(20));
                data.SelectionCount = Mathf.Clamp(temp, 1, 4);
            }
            else
            {
                data.SelectionCount = 1;
            }

            EditorGUILayout.EndHorizontal();

            data.foldout = EditorGUILayout.BeginFoldoutHeaderGroup(data.foldout, "Details");
            if (data.foldout && data.actionNodeType != ActionNodeType.NONE)
            {
                for (int i = 0; i < data.SelectionCount; i++)
                {
                    EditorGUILayout.LabelField($"{i}");
                    data.scripts[i] = EditorGUILayout.TextArea(data.scripts[i], data.actionNodeType == ActionNodeType.CHARACTER_SPEECH ? GUILayout.Height(50) : GUILayout.Height(20));
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
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