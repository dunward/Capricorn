using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class PlayBGMUnit : FadeUnit
    {
        public string bgm;

#if UNITY_EDITOR
        protected override string info => "Play BGM";

        public override void OnGUI(Rect rect, ref float height)
        {
            base.OnGUI(rect, ref height);

            var dropDownRect = new Rect(rect.x, rect.y + height, rect.width, UnityEditor.EditorGUIUtility.singleLineHeight);
            if (UnityEditor.EditorGUI.DropdownButton(dropDownRect, new GUIContent(bgm), FocusType.Passive))
            {
                ShowDropDown(dropDownRect);
            }

            height += UnityEditor.EditorGUIUtility.singleLineHeight;
        }

        private void ShowDropDown(Rect rect)
        {
            UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
            var bgmNames = Resources.Load<AudioDatabase>("AudioDatabase").bgms.Keys;

            foreach (var name in bgmNames.OrderBy(bgm => bgm))
            {
                menu.AddItem(new GUIContent(name), name == bgm, OnSelected, name);
            }

            menu.DropDown(rect);
        }

        private void OnSelected(object select)
        {
            bgm = select as string;
        }

        public override float GetHeight()
        {
            return base.GetHeight() + UnityEditor.EditorGUIUtility.singleLineHeight;
        }
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var info = Resources.Load<AudioDatabase>("AudioDatabase");
            var goRef = args[0] as Ref<GameObject>;
            
            Object.Destroy(goRef.Value);

            var go = Object.Instantiate(info.bgmPrefab);
            var audio = go.GetComponent<AudioSource>();

            go.name = bgm;

            var target = info.bgms[bgm];

            audio.clip = target.clip;
            audio.Play();

            var time = 0f;

            while (fade && time < elapsedTime)
            {
                var targetVolume = Mathf.Lerp(0, target.maxVolume, time / elapsedTime);
                time += Time.deltaTime;

                audio.volume = targetVolume;
                yield return null;
            }

            audio.volume = target.maxVolume;

            goRef.Value = go;
        }
    }
}