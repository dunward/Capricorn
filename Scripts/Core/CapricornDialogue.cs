using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Dunward.Capricorn
{
    [DisallowMultipleComponent]
    public class CapricornDialogue : MonoBehaviour
    {
        [SerializeField]
        private Object nameTarget;
        [SerializeField]
        private Object subNameTarget;
        [SerializeField]
        private Object scriptTarget;

        private TMP_Text name_TMP;
        private TMP_Text subName_TMP;
        private TMP_Text script_TMP;

        private Text name_UI;
        private Text subName_UI;
        private Text script_UI;

        public object NameTarget
        {
            get
            {
                if (name_TMP != null) return name_TMP;
                if (name_UI != null) return name_UI;
                return null;
            }
        }

        public object SubNameTarget
        {
            get
            {
                if (subName_TMP != null) return subName_TMP;
                if (subName_UI != null) return subName_UI;
                return null;
            }
        }

        public object ScriptTarget
        {
            get
            {
                if (script_TMP != null) return script_TMP;
                if (script_UI != null) return script_UI;
                return null;
            }
        }

        public void Initialize()
        {
            name_TMP = nameTarget.GetComponent<TMP_Text>();
            subName_TMP = subNameTarget.GetComponent<TMP_Text>();
            script_TMP = scriptTarget.GetComponent<TMP_Text>();

            name_UI = nameTarget.GetComponent<Text>();
            subName_UI = subNameTarget.GetComponent<Text>();
            script_UI = scriptTarget.GetComponent<Text>();
        }
    }
}