using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class DeleteCharacterUnit : FadeUnit
    {
        public string character;

#if UNITY_EDITOR
        protected override string info => "Delete Character";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);

            var dropDownRect = new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight);
            if (UnityEditor.EditorGUI.DropdownButton(dropDownRect, new GUIContent(character), FocusType.Passive))
            {
                ShowCharacterPopup(dropDownRect);
            }

            height += UnityEditor.EditorGUIUtility.singleLineHeight;
        }

        private void ShowCharacterPopup(Rect rect)
        {
            UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
            var characters = Resources.Load<CharacterDatabase>("CharacterDatabase").characters.Keys;

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
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var time = 0f;
            var map = args[0] as Dictionary<string, GameObject>;
            var go = map[character];
            var image = go.GetComponent<UnityEngine.UI.Image>();
            var sprite = go.GetComponent<SpriteRenderer>();

            while (fade && time < elapsedTime)
            {
                var targetColor = Color.Lerp(Color.white, Color.black, time / elapsedTime);
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
            
            Object.Destroy(go);
            map.Remove(character);
        }
    }
}