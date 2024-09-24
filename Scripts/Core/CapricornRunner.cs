using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json;
using TMPro;
using System.Collections;

namespace Dunward.Capricorn
{
    public class CapricornRunner
    {
        private GraphData graphData;

#region Test
        public TextMeshProUGUI nameTmp;
        public TextMeshProUGUI subNameTmp;
        public TextMeshProUGUI scriptTmp;

        public Button inputPanel;
#endregion
        
        public readonly NodeMainData startNode;
        private Dictionary<int, NodeMainData> nodes = new Dictionary<int, NodeMainData>();

        public CapricornRunner(string text)
        {
            graphData = JsonConvert.DeserializeObject<GraphData>(text);
            startNode = graphData.nodes.Find(node => node.nodeType == NodeType.Input);

            foreach (var node in graphData.nodes)
            {
                nodes.Add(node.id, node);
            }
        }

        public IEnumerator Run(MonoBehaviour monoBehaviour)
        {
            yield return RunTask(monoBehaviour).AsCoroutine();
        }

        private async Task RunTask(MonoBehaviour monoBehaviour)
        {
            var currentNode = startNode;

            while (true)
            {
                inputPanel.onClick.RemoveAllListeners();
                
                // TODO: Implement coroutine list here.
                // ...

                var action = CreateAction(currentNode.actionData);
                switch (action)
                {
                    case TextDisplayer textDisplayer:
                        inputPanel.onClick.AddListener(() => textDisplayer.Interaction());
                        await textDisplayer.Execute(nameTmp, subNameTmp, scriptTmp).AsTask(monoBehaviour);
                        break;
                    case SelectionDisplayer selectionDisplayer:
                        break;
                }

                if (currentNode.nodeType == NodeType.Output) return;
                currentNode = GetNextNode(currentNode);
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