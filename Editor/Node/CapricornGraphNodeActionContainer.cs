#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

namespace Dunward
{
    public class CapricornGraphNodeActionContainer
    {
        private CapricornGraphNodeMainContainer main;

        private ActionNodeType actionNodeType = ActionNodeType.NONE;

        private bool foldout = true;
        private List<string> scripts = new List<string>();

        private int _selectionCount = 1;
        private int selectionCount
        {
            get => _selectionCount;
            set
            {
                _selectionCount = value;
                UpdateOutputPort();
            }
        }

        private Color portColor = new Color(0.69f, 0.98f, 0.34f);

        public CapricornGraphNodeActionContainer(CapricornGraphNodeMainContainer main)
        {
            this.main = main;

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
            actionNodeType = (ActionNodeType)EditorGUILayout.EnumPopup(actionNodeType);

            if (actionNodeType == ActionNodeType.TEACHER_SPEECH)
            {
                var temp = EditorGUILayout.IntField(selectionCount, GUILayout.Width(20));
                selectionCount = Mathf.Clamp(temp, 1, 4);
            }
            else
            {
                selectionCount = 1;
            }

            EditorGUILayout.EndHorizontal();

            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Details");
            if (foldout && actionNodeType != ActionNodeType.NONE)
            {
                for (int i = 0; i < selectionCount; i++)
                {
                    EditorGUILayout.LabelField($"{i}");
                    scripts[i] = EditorGUILayout.TextArea(scripts[i], actionNodeType == ActionNodeType.CHARACTER_SPEECH ? GUILayout.Height(50) : GUILayout.Height(20));
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void UpdateOutputPort()
        {
            if (main.outputContainer.childCount > selectionCount)
            {
                for (int i = main.outputContainer.childCount - 1; i >= selectionCount; i--)
                {
                    var port = main.outputContainer[i] as Port;
                    main.graphView.DeleteElements(port.connections);
                    main.outputContainer.RemoveAt(i);
                    scripts.RemoveAt(i);
                }
            }
            else if (main.outputContainer.childCount < selectionCount)
            {
                for (int i = main.outputContainer.childCount; i < selectionCount; i++)
                {
                    var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                    outputPort.portName = $"{i}";
                    outputPort.portColor = portColor;
                    main.outputContainer.Add(outputPort);
                    scripts.Add(string.Empty);
                }
            }
        }
    }
}
#endif