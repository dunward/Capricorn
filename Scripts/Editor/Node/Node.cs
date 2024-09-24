using UnityEngine;

namespace Dunward.Capricorn
{
    public abstract class Node : UnityEditor.Experimental.GraphView.Node
    {
        public readonly GraphView graphView;
        public readonly NodeMainContainer main;

        protected int id;
        public int ID
        {
            get => id;
        }

        public Node(GraphView graphView, int id, float x, float y)
        {
            this.graphView = graphView;
            this.id = id;

            titleContainer.AddToClassList("capricorn-title-container");
            mainContainer.Insert(1, extensionContainer);

            SetupTitleContainer();
            SetPosition(new Rect(x, y, 0, 0));

            main = new NodeMainContainer(this);

            RefreshExpandedState();
        }

        public Node(GraphView graphView, int id, Vector2 mousePosition) : this(graphView, id, mousePosition.x, mousePosition.y)
        {

        }

        public Node(GraphView graphView, NodeMainData mainData) : this(graphView, mainData.id, mainData.x, mainData.y)
        {
            UpdateSubContainers(mainData);
        }

        public virtual NodeMainData GetMainData()
        {
            main.action.SerializeConnections();

            var mainData = new NodeMainData();
            mainData.id = id;
            mainData.x = GetPosition().x;
            mainData.y = GetPosition().y;
            mainData.actionData = main.action.data;

            return mainData;
        }

        protected abstract void SetupTitleContainer();
        
        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.action.DeserializeConnections(mainData.actionData);
        }
    }
}