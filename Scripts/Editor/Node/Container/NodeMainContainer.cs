#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public class NodeMainContainer
    {
        public readonly BaseNode parent;
        public readonly VisualElement coroutineContainer;
        public readonly VisualElement actionContainer;

        public readonly NodeCoroutineContainer coroutine;
        public readonly NodeActionContainer action;
        
        public VisualElement mainContainer => parent.mainContainer;
        public VisualElement inputContainer => parent.inputContainer;
        public VisualElement outputContainer => parent.outputContainer;

        public GraphView graphView => parent.graphView;

        public NodeMainContainer(BaseNode node)
        {
            parent = node;
            var mainContainer = new VisualElement();
            mainContainer.AddToClassList("capricorn-main-container");

            coroutineContainer = new VisualElement();
            actionContainer = new VisualElement();

            coroutineContainer.AddToClassList("capricorn-coroutine-container");
            actionContainer.AddToClassList("capricorn-action-container");

            coroutine = new NodeCoroutineContainer(this);
            action = new NodeActionContainer(this);

            mainContainer.Add(coroutineContainer);
            mainContainer.Add(actionContainer);

            parent.extensionContainer.Add(mainContainer);
        }
    }
}
#endif