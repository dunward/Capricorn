using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json;
using TMPro;

namespace Dunward.Capricorn
{
    public class CapricornRunner
    {
        private GraphData graphData;
        
        private MonoBehaviour target;

#region Test
        public TextMeshProUGUI nameTmp;
        public TextMeshProUGUI subNameTmp;
        public TextMeshProUGUI scriptTmp;

        public Button inputPanel;
#endregion
        
        public readonly NodeMainData startNode;
        private Dictionary<int, NodeMainData> nodes = new Dictionary<int, NodeMainData>();

        public CapricornRunner(string text, MonoBehaviour target)
        {
            this.target = target;

            graphData = JsonConvert.DeserializeObject<GraphData>(text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            startNode = graphData.nodes.Find(node => node.nodeType == NodeType.Input);

            foreach (var node in graphData.nodes)
            {
                nodes.Add(node.id, node);
            }
        }

        public IEnumerator Run()
        {
            var currentNode = startNode;

            while (true)
            {
                inputPanel.onClick.RemoveAllListeners();

                yield return RunCoroutine(currentNode.coroutineData, target);

                var action = CreateAction(currentNode.actionData);
                yield return RunAction(action);

                if (currentNode.nodeType == NodeType.Output) yield break;
                currentNode = GetNextNode(currentNode);
            }
        }

        private IEnumerator RunCoroutine(NodeCoroutineData data, MonoBehaviour target)
        {
            foreach (var coroutine in data.coroutines)
            {
                if (coroutine.isWaitingUntilFinish)
                {
                    yield return target.StartCoroutine(coroutine.Execute());
                }
                else
                {
                    target.StartCoroutine(coroutine.Execute());
                }
            }
        }

        private IEnumerator RunAction(ActionPlayer action)
        {
            switch (action)
            {
                case TextDisplayer textDisplayer:
                    inputPanel.onClick.AddListener(() => textDisplayer.Interaction());
                    yield return textDisplayer.Execute(nameTmp, subNameTmp, scriptTmp);
                    break;
                case SelectionDisplayer selectionDisplayer:
                    break;
            }
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