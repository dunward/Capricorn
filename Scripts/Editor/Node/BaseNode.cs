#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Dunward.Capricorn
{
    public abstract class BaseNode : Node
    {
        public readonly GraphView graphView;
        public readonly NodeMainContainer main;

        protected string customTitle;
        protected NodeType nodeType;

        protected int id;
        public int ID
        {
            get => id;
        }

        public BaseNode()
        {
            titleContainer.AddToClassList("capricorn-title-container");
            mainContainer.Insert(1, extensionContainer);

            main = new NodeMainContainer(this);

            SetupTitleContainer();
            RefreshExpandedState();
        }

        public BaseNode(GraphView graphView, int id, float x, float y) : this()
        {
            this.graphView = graphView;
            this.id = id;

            SetPosition(new Rect(x, y, 0, 0));
        }

        public BaseNode(GraphView graphView, int id, Vector2 mousePosition) : this(graphView, id, mousePosition.x, mousePosition.y)
        {

        }

        public BaseNode(GraphView graphView, NodeMainData mainData) : this(graphView, mainData.id, mainData.x, mainData.y)
        {
            customTitle = mainData.title;
            UpdateSubContainers(mainData);
            Repaint();
        }

        public virtual NodeMainData GetMainData()
        {
            main.action.SerializeConnections();

            var mainData = new NodeMainData();
            mainData.title = customTitle;
            mainData.id = id;
            mainData.x = GetPosition().x;
            mainData.y = GetPosition().y;
            mainData.nodeType = nodeType;
            mainData.coroutineData = main.coroutine.CoroutineData;
            mainData.actionData = main.action.data;

            return mainData;
        }

        protected abstract void SetupTitleContainer();

        protected virtual void Repaint()
        {

        }

        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.action.DeserializeConnections(mainData.actionData);
        }
    }
}
#endif