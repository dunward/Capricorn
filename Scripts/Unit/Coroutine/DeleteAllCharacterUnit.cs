using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Dunward.Capricorn
{
    public class DeleteAllCharacterUnit : FadeUnit
    {
#if UNITY_EDITOR
        protected override string info => "Delete All Character";
#endif

        private Dictionary<GameObject, Color> targetColors = new Dictionary<GameObject, Color>();

        public override IEnumerator Execute(params object[] args)
        {
            var time = 0f;
            var map = args[0] as Dictionary<string, GameObject>;

            if (fade)
            {
                foreach (var pair in map)
                {
                    var image = pair.Value.GetComponent<SpriteRenderer>();
                    var sprite = pair.Value.GetComponent<UnityEngine.UI.Image>();

                    if (image != null)
                    {
                        targetColors.Add(pair.Value, image.color);
                    }
                    else if (sprite != null)
                    {
                        targetColors.Add(pair.Value, sprite.color);
                    }
                }
            }

            while (fade && time < elapsedTime)
            {
                time += Time.deltaTime;

                foreach (var pair in map)
                {
                    var targetColor = Color.Lerp(targetColors[pair.Value], Color.black, time / elapsedTime);
                    var image = pair.Value.GetComponent<SpriteRenderer>();
                    var sprite = pair.Value.GetComponent<UnityEngine.UI.Image>();

                    if (image != null)
                    {
                        image.color = targetColor;
                    }
                    else if (sprite != null)
                    {
                        sprite.color = targetColor;
                    }
                }

                yield return null;
            }
            
            foreach (var pair in map)
            {
                Object.Destroy(pair.Value);
            }
            
            map.Clear();
        }
    }
}