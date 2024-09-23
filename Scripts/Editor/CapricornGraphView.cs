#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

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
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new DragAndDropManipulator(data => Load(data)));
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

        public void Load(string json)
        {
            ClearGraph();

            var data = JsonConvert.DeserializeObject<CapricornGraphData>(json);
            foreach (var nodeData in data.nodes)
            {
                var node = new CapricornGraphNode(this, nodeData);
                AddElement(node);
            }

            ConnectDeserializeNodes();
            lastNodeID = data.nodes.Max(n => n.id);
        }
        
        private void ConnectDeserializeNodes()
        {
            foreach (var node in nodes)
            {
                var cn = node as CapricornGraphNode;
                for (int i = 0; i < cn.main.action.data.connections.Count; i++)
                {
                    var connection = cn.main.action.data.connections[i];
                    var outputPort = cn.outputContainer[i] as Port;
                    var inputPort = nodes.Where(n => n is CapricornGraphNode)
                                        .Cast<CapricornGraphNode>()
                                        .First(n => n.ID == connection).inputContainer[0] as Port;

                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
            }
        }

        private void ClearGraph()
        {
            DeleteElements(nodes.ToList());
        }

        private void AddNode(Vector2 position)
        {
            var node = new CapricornGraphNode(this, ++lastNodeID, position);
            AddElement(node);
        }

        private class DragAndDropManipulator : PointerManipulator
        {
            private Object item;
            private readonly System.Action<string> onLoadGraph;

            public DragAndDropManipulator(System.Action<string> onLoadGraph)
            {
                this.onLoadGraph = onLoadGraph;
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

            private void OnDragPerform(DragPerformEvent _)
            {
                var textAsset = item as TextAsset;
                onLoadGraph?.Invoke(textAsset.text);
            }

            private bool IsSingleTextAsset()
            {
                return item is TextAsset && DragAndDrop.objectReferences.Length == 1;
            }
        }
    }
}
#endif