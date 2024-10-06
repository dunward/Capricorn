using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class DeleteForegroundUnit : FadeUnit
    {
        public string backgroundImage;

#if UNITY_EDITOR
        protected override string info => "Delete Foreground";
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var goRef = args[0] as Ref<GameObject>;
            var image = goRef.Value.GetComponent<UnityEngine.UI.Image>();
            var sprite = goRef.Value.GetComponent<SpriteRenderer>();

            var time = 0f;

            while (fade && time < elapsedTime)
            {
                var targetColor = Color.Lerp(Color.white, Color.clear, lerpCurve.Evaluate(time / elapsedTime));
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

            Object.Destroy(goRef.Value);
            goRef.Value = null;
        }
    }
}