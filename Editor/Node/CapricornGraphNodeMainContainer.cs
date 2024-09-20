#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dunward
{
    public class CapricornGraphNodeMainContainer
    {
        public readonly Node parent;
        public readonly VisualElement actionContainer;
        public readonly VisualElement coroutineContainer;
        
        public VisualElement inputContainer => parent.inputContainer;
        public VisualElement outputContainer => parent.outputContainer;

        public CapricornGraphNodeMainContainer(Node node)
        {
            parent = node;
            var mainContainer = new VisualElement();
            mainContainer.AddToClassList("capricorn-main-container");

            coroutineContainer = new VisualElement();
            actionContainer = new VisualElement();

            coroutineContainer.AddToClassList("capricorn-coroutine-container");
            actionContainer.AddToClassList("capricorn-action-container");

            new CapricornGraphNodeCoroutineContainer(this);
            new CapricornGraphNodeActionContainer(this);

            mainContainer.Add(coroutineContainer);
            mainContainer.Add(actionContainer);

            parent.extensionContainer.Add(mainContainer);
        }
    }
}
#endif