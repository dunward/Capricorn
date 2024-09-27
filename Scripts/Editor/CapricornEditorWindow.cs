using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.VisualScripting;

namespace Dunward.Capricorn
{
    public class CapricornEditorWindow : EditorWindow
    {
        public StyleSheet graphStyle;

        private GraphView graphView;

        private string filePath = null;

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

            GUILayout.Space(5);

            if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            {
                if (EditorUtility.OpenFilePanel("Load Graph", "", "json") is string path)
                {
                    graphView.Load(path);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                graphView.Save();
            }

            GUILayout.Space(5);
            
            if (GUILayout.Button("Save As...", EditorStyles.toolbarButton))
            {
                graphView.SaveAs();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void AddGraphView()
        {
            var content = new VisualElement();
            graphView = new GraphView(filePath);

            graphView.onChangeFilePath += (path) =>
            {
                filePath = path;
            };

            content.styleSheets.Add(graphStyle);
            content.name = "content";
            content.Add(graphView);
            rootVisualElement.Add(content);
        }
    }
}
