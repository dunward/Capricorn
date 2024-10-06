using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dunward.Capricorn
{
    [System.Serializable]
    public class StopBGMUnit : FadeUnit
    {
#if UNITY_EDITOR
        protected override string info => "Stop BGM";
#endif

        public override IEnumerator Execute(params object[] args)
        {
            var goRef = args[0] as Ref<GameObject>;
            
            var audio = goRef.Value.GetComponent<AudioSource>();
            var startVolume = audio.volume;

            var time = 0f;

            while (fade && time < elapsedTime)
            {
                var targetVolume = Mathf.Lerp(startVolume, 0, lerpCurve.Evaluate(time / elapsedTime));
                time += Time.deltaTime;

                audio.volume = targetVolume;
                yield return null;
            }
            
            Object.Destroy(goRef.Value);
        }
    }
}