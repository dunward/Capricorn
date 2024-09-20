
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Newtonsoft.Json;

namespace Dunward
{
    public class CapricornGraphNode : Node
    {
        public readonly CapricornGraphView graphView;
        public readonly CapricornGraphNodeMainContainer main;

        public CapricornGraphNode(CapricornGraphView graphView, int id, float x, float y)
        {
            this.graphView = graphView;
            mainContainer.Insert(1, extensionContainer);
            title = $"{id}";
            SetPosition(new Rect(x, y, 0, 0));

            main = new CapricornGraphNodeMainContainer(this);

            RefreshExpandedState();
        }

        public CapricornGraphNode(CapricornGraphView graphView, int id, Vector2 mousePosition) : this(graphView, id, mousePosition.x, mousePosition.y)
        {

        }

        public CapricornGraphNodeMainData GetMainData()
        {
            var mainData = new CapricornGraphNodeMainData();
            mainData.actionData = main.action.data;
            return mainData;
        }
    }
}