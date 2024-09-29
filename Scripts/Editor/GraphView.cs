#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

using Newtonsoft.Json;

namespace Dunward.Capricorn
{
    public class GraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        private NodeSearchWindow nodeSearchWindow;
        private Label titleLabel;

        private InputNode inputNode; // This node is the start point of the graph. It is not deletable and unique.
        private int lastNodeID = 0;

        private string filePath = null;

        private JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public System.Action<string> onChangeFilePath;

        public GraphView(string filePath = null)
        {
            titleLabel = new Label(string.IsNullOrEmpty(filePath) ? "New Graph" : System.IO.Path.GetFileName(filePath));
            titleLabel.AddToClassList("capricorn-graph-title");
            contentContainer.Add(titleLabel);

            if (nodeSearchWindow == null)
                nodeSearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();

            nodeSearchWindow.onSelectNode = (type, position) => 
            {
                var localPosition = contentViewContainer.WorldToLocal(position);
                var node = (BaseNode)System.Activator.CreateInstance(type, this, ++lastNodeID, localPosition);
                AddElement(node);
            };

            if (string.IsNullOrEmpty(filePath))
            {
                inputNode = new InputNode(this, -1, 0, 0);
                AddElement(inputNode);
            }
            else
            {
                Load(filePath);
            }

            UnityEditor.Compilation.CompilationPipeline.compilationStarted += (_) => Save();

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new DragAndDropManipulator(data => Load(data)));
            this.AddManipulator(new ContextualMenuManipulator(evt => evt.menu.AppendAction("Add Node",
                                    (action) => SearchWindow.Open(new SearchWindowContext(action.eventInfo.mousePosition), nodeSearchWindow),
                                    DropdownMenuAction.AlwaysEnabled)));
            
            this.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.S && evt.actionKey)
                {
                    Save();
                    evt.StopPropagation();
                }
            });
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
            var data = new GraphData();
            data.position = viewTransform.position;
            data.zoomFactor = viewTransform.scale.x;

            foreach (var node in nodes)
            {
                if (node is BaseNode capricornGraphNode)
                {
                    data.nodes.Add(capricornGraphNode.GetMainData());
                }
            }

            return JsonConvert.SerializeObject(data, settings);
        }

        public void Load(string path)
        {
            ClearGraph();

            filePath = path;
            UpdateTitleLabel();
            onChangeFilePath?.Invoke(path);

            var json = System.IO.File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<GraphData>(json, settings);
            viewTransform.position = data.position;
            viewTransform.scale = new Vector3(data.zoomFactor, data.zoomFactor, 1);

            foreach (var nodeData in data.nodes)
            {
                switch (nodeData.nodeType)
                {
                    case NodeType.Input:
                        inputNode = new InputNode(this, nodeData);
                        AddElement(inputNode);
                        break;
                    case NodeType.Output:
                        var outputNode = new OutputNode(this, nodeData);
                        AddElement(outputNode);
                        break;
                    case NodeType.Connector:
                    default:
                        var node = new ConnectorNode(this, nodeData);
                        AddElement(node);
                        break;
                }
            }

            ConnectDeserializeNodes();
            lastNodeID = data.nodes.Max(n => n.id);
        }
     
        public void Save()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SaveAs();
                return;
            }

            var json = SerializeGraph();
            System.IO.File.WriteAllText(filePath, json);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public void SaveAs()
        {
            var path = EditorUtility.SaveFilePanel("Save Graph", "", "graph", "json");
            if (string.IsNullOrEmpty(path)) return;

            UpdateTitleLabel();
            onChangeFilePath?.Invoke(path);
            filePath = path;
            Save();
        }

        private void ConnectDeserializeNodes()
        {
            foreach (var node in nodes)
            {
                var baseNode = node as BaseNode;
                for (int i = 0; i < baseNode.main.action.data.connections.Count; i++)
                {
                    var connection = baseNode.main.action.data.connections[i];
                    var outputPort = baseNode.outputContainer[i] as Port;
                    var inputPort = nodes.Where(n => n is BaseNode)
                                        .Cast<BaseNode>()
                                        .First(n => n.ID == connection).inputContainer[0] as Port;

                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
            }
        }

        private void ClearGraph()
        {
            DeleteElements(nodes.ToList());
            DeleteElements(edges.ToList());
        }

        private void UpdateTitleLabel()
        {
            titleLabel.text = string.IsNullOrEmpty(filePath) ? "New Graph" : System.IO.Path.GetFileName(filePath);
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
                EditorUtility.DisplayProgressBar("Capricorn", "Load Graph...", 0.112f);
                var textAsset = item as TextAsset;
                var path = AssetDatabase.GetAssetPath(textAsset);
                onLoadGraph?.Invoke(path);
                EditorUtility.ClearProgressBar();
            }

            private bool IsSingleTextAsset()
            {
                return item is TextAsset && DragAndDrop.objectReferences.Length == 1;
            }
        }
    }
}
#endif