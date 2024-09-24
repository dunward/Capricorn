using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Dunward.Capricorn
{
    public class CapricornRunner : MonoBehaviour
    {
        public TextAsset textAsset;
        private GraphData graphData;
        
        private NodeMainData startNode;
        private Dictionary<int, NodeMainData> nodes = new Dictionary<int, NodeMainData>();

        public void Awake()
        {
            graphData = JsonConvert.DeserializeObject<GraphData>(textAsset.text);
            startNode = graphData.nodes.Find(node => node.nodeType == NodeType.Input);

            foreach (var node in graphData.nodes)
            {
                nodes.Add(node.id, node);
            }

            StartCoroutine(Run());
        }

        public IEnumerator Run()
        {
            var currentNode = startNode;

            while (true)
            {
                // TODO: Implement coroutine list here.
                var action = CreateAction(currentNode.actionData);
                yield return action.Run();

                if (currentNode.nodeType == NodeType.Output) yield break;

                // Test delay
                yield return new WaitForSeconds(1);
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
                    return new ActionPlayer();
            }
        }
    }
}