using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class ShowCharacterUnit : FadeUnit
    {
        public string character;
#if UNITY_EDITOR
        protected override string info => "Show Character";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);

            if (EditorGUI.DropdownButton(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), new GUIContent(character), FocusType.Passive))
            {
                ShowCharacterPopup(rect);
            }
            // if (GUI.Button(new Rect(rect.x, rect.y + height, rect.width, height), "Select Character"))
            // {
            //     ShowCharacterPopup(rect);
            // }
            // // test string to enums
            // // enum change
            // EditorGUI.EnumPopup(new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight), "Character", 0);
        }

        private void ShowCharacterPopup(Rect rect)
        {
            GenericMenu menu = new GenericMenu();
                var characters = new List<string> { "Character 1", "Character 2", "Character 3" };

            foreach (var c in characters)
            {
                menu.AddItem(new GUIContent(c), c == character, OnCharacterSelected, c);
            }

            menu.DropDown(rect);
        }

        private void OnCharacterSelected(object c)
        {
            character = c as string;
        }

        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var obj = args[0] as GameObject;
            Debug.LogError(obj.name);
            yield return new WaitForSeconds(elapsedTime);
        }
    }
}