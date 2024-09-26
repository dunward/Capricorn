using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ShowCharacterUnit : FadeUnit
    {
        public string character;

        public CapricornVector2 position;

        public float scale = 1;

#if UNITY_EDITOR
        protected override string info => "Show Character";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);

            if (UnityEditor.EditorGUI.DropdownButton(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), new GUIContent(character), FocusType.Passive))
            {
                ShowCharacterPopup(rect);
            }

            height += UnityEditor.EditorGUIUtility.singleLineHeight;

            position = UnityEditor.EditorGUI.Vector2Field(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight * 2), "Position", position);

            height += UnityEditor.EditorGUIUtility.singleLineHeight * 2;

            scale = Mathf.Clamp(UnityEditor.EditorGUI.FloatField(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Scale", scale), 0, float.MaxValue);

            height += UnityEditor.EditorGUIUtility.singleLineHeight;
        }

        private void ShowCharacterPopup(Rect rect)
        {
            UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
            var characters = new List<string> { "arona", "hosino", "ako" };

            foreach (var c in characters.OrderBy(c => c))
            {
                menu.AddItem(new GUIContent(c), c == character, OnCharacterSelected, c);
            }

            menu.DropDown(rect);
        }

        private void OnCharacterSelected(object select)
        {
            character = select as string;
        }

        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight * 4;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var chars = Resources.Load<CharacterDatabase>("CharacterDatabase").characters;
            var prefab = chars.Find(c => c.name == character).prefab;
            var parent = args[0] as Transform;

            var go = Object.Instantiate(prefab, parent);

            if (go.transform is RectTransform)
            {
                var rt = go.transform as RectTransform;
                rt.anchoredPosition = position;
                rt.localScale = new Vector3(scale, scale, 1);
            }
            else
            {
                go.transform.position = position;
                go.transform.localScale = new Vector3(scale, scale, 1);
            }
            
            var time = 0f;
            var image = go.GetComponent<UnityEngine.UI.Image>();
            var sprite = go.GetComponent<SpriteRenderer>();

            while (fade && time < elapsedTime)
            {
                time += Time.deltaTime;

                if (image != null)
                {
                    image.color = Color.Lerp(Color.black, Color.white, time / elapsedTime);
                }
                else if (sprite != null)
                {
                    sprite.color = Color.Lerp(Color.black, Color.white, time / elapsedTime);
                }

                yield return null;
            }
        }
    }
}