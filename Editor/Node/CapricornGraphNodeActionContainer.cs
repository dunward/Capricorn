#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dunward
{
    public class CapricornGraphNodeActionContainer
    {
        public CapricornGraphNodeActionContainer(CapricornGraphNodeMainContainer main)
        {
            var enumField = new EnumField(ActionNodeType.NONE);
            main.actionContainer.Add(enumField);

            var input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            var output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            input.portName = string.Empty;
            output.portName = string.Empty;
            input.portColor = new Color(0.69f, 0.98f, 0.34f);
            output.portColor = new Color(0.69f, 0.98f, 0.34f);

            main.inputContainer.Add(input);
            main.outputContainer.Add(output);
        }
    }
}
#endif