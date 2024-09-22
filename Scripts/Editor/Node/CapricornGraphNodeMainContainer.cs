#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace Dunward
{
    public class CapricornGraphNodeMainContainer
    {
        public readonly CapricornGraphNode parent;
        public readonly VisualElement coroutineContainer;
        public readonly VisualElement actionContainer;

        public readonly CapricornGraphNodeCoroutineContainer coroutine;
        public readonly CapricornGraphNodeActionContainer action;
        
        public VisualElement mainContainer => parent.mainContainer;
        public VisualElement inputContainer => parent.inputContainer;
        public VisualElement outputContainer => parent.outputContainer;

        public CapricornGraphView graphView => parent.graphView;

        public CapricornGraphNodeMainContainer(CapricornGraphNode node)
        {
            parent = node;
            var mainContainer = new VisualElement();
            mainContainer.AddToClassList("capricorn-main-container");

            coroutineContainer = new VisualElement();
            actionContainer = new VisualElement();

            coroutineContainer.AddToClassList("capricorn-coroutine-container");
            actionContainer.AddToClassList("capricorn-action-container");

            coroutine = new CapricornGraphNodeCoroutineContainer(this);
            action = new CapricornGraphNodeActionContainer(this);

            mainContainer.Add(coroutineContainer);
            mainContainer.Add(actionContainer);

            parent.extensionContainer.Add(mainContainer);
        }
    }
}
#endif