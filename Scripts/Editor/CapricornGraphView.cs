using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

using Newtonsoft.Json;

namespace Dunward
{
    public class CapricornGraphView : GraphView
    {
        private int lastNodeID = 0;

        public CapricornGraphView()
        {
            var node = new CapricornGraphNode(this, -1, new Vector2(100, 200));
            AddElement(node);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new DragAndDropManipulator());

            this.AddManipulator(new ContextualMenuManipulator(evt => evt.menu.AppendAction("Add Node", action => AddNode(action.eventInfo.localMousePosition), DropdownMenuAction.AlwaysEnabled)));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public string SerializeGraph()
        {
            var data = new CapricornGraphData();
            foreach (var node in nodes)
            {
                if (node is CapricornGraphNode capricornGraphNode)
                {
                    data.nodes.Add(capricornGraphNode.GetMainData());
                }
            }
            return JsonConvert.SerializeObject(data);
        }

        private void AddNode(Vector2 position)
        {
            var node = new CapricornGraphNode(this, lastNodeID, position);
            AddElement(node);
            lastNodeID++;
        }

        private class DragAndDropManipulator : PointerManipulator
        {
            private Object item;

            public DragAndDropManipulator()
            {
                
            }

            protected override void RegisterCallbacksOnTarget()
            {
                target.RegisterCallback<DragEnterEvent>(OnDragEnter);
                target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
                target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
                target.RegisterCallback<DragPerformEvent>(OnDragPerform);
            }

            protected override void UnregisterCallbacksFromTarget()
            {
                target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
                target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
                target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
                target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
            }

            private void OnDragEnter(DragEnterEvent _)
            {
                item = DragAndDrop.objectReferences[0];
            }

            private void OnDragLeave(DragLeaveEvent _)
            {
                item = null;
            }

            private void OnDragUpdated(DragUpdatedEvent _)
            {
                DragAndDrop.visualMode = IsSingleTextAsset() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
            }

            private void OnDragPerform(DragPerformEvent evt)
            {
                var textAsset = item as TextAsset;
                Debug.LogError(textAsset.text);
            }

            private bool IsSingleTextAsset()
            {
                return item is TextAsset && DragAndDrop.objectReferences.Length == 1;
            }
        }
    }
}