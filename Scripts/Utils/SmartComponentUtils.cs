using UnityEngine.UI;

using TMPro;

namespace Dunward.Capricorn
{
    public static class SmartComponentUtils
    {
        public static void SetText(this object textComponent, string text)
        {
            switch (textComponent)
            {
                case TMP_Text tmpText:
                    tmpText.text = text;
                    break;
                case Text uiText:
                    uiText.text = text;
                    break;
                case null:
                    break;
                default:
                    throw new System.Exception("TextUtils.SetText() is not implemented for this type.");
            }
        }

        public static void AppendText(this object textComponent, char text)
        {
            switch (textComponent)
            {
                case TMP_Text tmpText:
                    tmpText.text += text;
                    break;
                case Text uiText:
                    uiText.text += text;
                    break;
                case null:
                    break;
                default:
                    throw new System.Exception("TextUtils.AppendText() is not implemented for this type.");
            }
        }

        public static string GetText(this object textComponent)
        {
            switch (textComponent)
            {
                case TMP_Text tmpText:
                    return tmpText.text;
                case Text uiText:
                    return uiText.text;
                case null:
                    return null;
                default:
                    throw new System.Exception("TextUtils.GetText() is not implemented for this type.");
            }
        }
    }
}