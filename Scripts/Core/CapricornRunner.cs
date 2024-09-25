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

            graphData = JsonConvert.DeserializeObject<GraphData>(text);
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
                
                // TODO: Implement coroutine list here.
                // ...

                var action = CreateAction(currentNode.actionData);
                switch (action)
                {
                    case TextDisplayer textDisplayer:
                        inputPanel.onClick.AddListener(() => textDisplayer.Interaction());
                        yield return textDisplayer.Execute(nameTmp, subNameTmp, scriptTmp);
                        break;
                    case SelectionDisplayer selectionDisplayer:
                        break;
                }

                if (currentNode.nodeType == NodeType.Output) yield break;
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