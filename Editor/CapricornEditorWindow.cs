using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Dunward
{
    public class CapricornEditorWindow : EditorWindow
    {
        public StyleSheet graphStyle;

        [MenuItem("Constellation/Capricorn/Graph View")]
        public static void ShowExample()
        {
            var window = GetWindow<CapricornEditorWindow>();
            window.titleContent = new GUIContent("Capricorn Novel Editor");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var imguiContainer = new IMGUIContainer(CreateToolbar);

            root.Add(imguiContainer);

            AddGraphView();
        }

        public void CreateToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                Debug.Log("Save clicked");
            }

            GUILayout.Space(5);
            
            if (GUILayout.Button("Save As...", EditorStyles.toolbarButton))
            {
                Debug.Log("Save As clicked");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void AddGraphView()
        {
            var content = new VisualElement();
            var graphView = new CapricornGraphView();
            content.styleSheets.Add(graphStyle);
            content.name = "content";
            content.Add(graphView);
            rootVisualElement.Add(content);
        }
    }
}
