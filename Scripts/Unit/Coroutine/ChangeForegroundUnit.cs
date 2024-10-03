using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ChangeForegroundUnit : FadeUnit
    {
        public string backgroundImage;

#if UNITY_EDITOR
        protected override string info => "Change Foreground";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);

            var dropDownRect = new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight);
            if (UnityEditor.EditorGUI.DropdownButton(dropDownRect, new GUIContent(backgroundImage), FocusType.Passive))
            {
                ShowBackgroundPopup(dropDownRect);
            }

            height += UnityEditor.EditorGUIUtility.singleLineHeight;
        }

        private void ShowBackgroundPopup(Rect rect)
        {
            UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
            var characters = Resources.Load<BackgroundDatabase>("BackgroundDatabase").backgrounds.Keys;

            foreach (var name in characters.OrderBy(c => c))
            {
                menu.AddItem(new GUIContent(name), name == backgroundImage, OnCharacterSelected, name);
            }

            menu.DropDown(rect);
        }

        private void OnCharacterSelected(object select)
        {
            backgroundImage = select as string;
        }

        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var database = Resources.Load<BackgroundDatabase>("BackgroundDatabase");
            var background = database.backgrounds[backgroundImage];
            var prefab = database.backgroundPrefab;

            var parent = args[0] as Transform;
            
            var go = Object.Instantiate(prefab, parent);
            go.name = backgroundImage;

            var time = 0f;
            var image = go.GetComponent<UnityEngine.UI.Image>();
            var sprite = go.GetComponent<SpriteRenderer>();

            if (image != null)
            {
                image.sprite = background;
            }
            else if (sprite != null)
            {
                sprite.sprite = background;
            }

            while (fade && time < elapsedTime)
            {
                var targetColor = new Color(1, 1, 1, time / elapsedTime);
                time += Time.deltaTime;

                if (image != null)
                {
                    image.color = targetColor;
                }
                else if (sprite != null)
                {
                    sprite.color = targetColor;
                }

                yield return null;
            }

            if (image != null)
            {
                image.color = Color.white;
            }
            else if (sprite != null)
            {
                sprite.color = Color.white;
            }

            var goRef = args[1] as Ref<GameObject>;
            Object.Destroy(goRef.Value);
            goRef.Value = go;
        }
    }
}