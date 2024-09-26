using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json;
using TMPro;

namespace Dunward.Capricorn
{
    public partial class CapricornRunner
    {
        internal GraphData graphData;
        
        internal MonoBehaviour target;
        internal Dictionary<int, NodeMainData> nodes = new Dictionary<int, NodeMainData>();
        
        public delegate IEnumerator CoroutineDelegate(CoroutineUnit unit);
        public event CoroutineDelegate AddCustomCoroutines;


        public NodeMainData StartNode
        {
            get => graphData.nodes.Find(node => node.nodeType == NodeType.Input);
        }

        // public CapricornRunner(string text, MonoBehaviour target)
        // {
        //     this.target = target;

        //     graphData = JsonConvert.DeserializeObject<GraphData>(text, new JsonSerializerSettings
        //     {
        //         TypeNameHandling = TypeNameHandling.Auto
        //     });
        //     startNode = graphData.nodes.Find(node => node.nodeType == NodeType.Input);

        //     foreach (var node in graphData.nodes)
        //     {
        //         nodes.Add(node.id, node);
        //     }
        // }

        public IEnumerator Run()
        {
            var currentNode = StartNode;

            while (true)
            {
                yield return RunCoroutine(currentNode.coroutineData, target);

                var action = CreateAction(currentNode.actionData);
                yield return RunAction(action);

                if (currentNode.nodeType == NodeType.Output) yield break;
                currentNode = GetNextNode(currentNode);
            }
        }

        public System.Action onInteraction;

        private IEnumerator RunCoroutine(NodeCoroutineData data, MonoBehaviour target)
        {
            foreach (var coroutine in data.coroutines)
            {
                if (coroutine.isWaitingUntilFinish)
                {
                    yield return target.StartCoroutine(ExecuteCoroutine(coroutine));
                }
                else
                {
                    target.StartCoroutine(ExecuteCoroutine(coroutine));
                }
            }
        }

        private IEnumerator ExecuteCoroutine(CoroutineUnit unit)
        {
            switch (unit)
            {
                case WaitUnit waitUnit:
                    yield return waitUnit.Execute();
                    break;
                default:
                    if (AddCustomCoroutines != null)
                    {
                        foreach (CoroutineDelegate coroutine in AddCustomCoroutines.GetInvocationList())
                        {
                            yield return coroutine(unit);
                        }
                    }
                    break;
            }
        }

        private IEnumerator RunAction(ActionPlayer action)
        {
            switch (action)
            {
                case TextDisplayer textDisplayer:
                    onInteraction += textDisplayer.Interaction;
                    yield return textDisplayer.Execute(nameTarget, subNameTarget, scriptTarget);
                    break;
                case SelectionDisplayer selectionDisplayer:
                    break;
            }

            onInteraction = null;
        }

        private NodeMainData GetNextNode(NodeMainData node)
        {
            var nextConnection = node.actionData.connections[0];
            return nodes[nextConnection];
        }
        
        private ActionPlayer CreateAction(NodeActionData actionData)
        {
            switch (actionData.actionNodeType)
            {
                case ActionNodeType.CHARACTER_SCRIPT:
                    return new TextDisplayer(actionData);
                case ActionNodeType.USER_SCRIPT:
                    return new SelectionDisplayer(actionData);
                default:
                    return new ActionPlayer(actionData);
            }
        }
    }
}