#if UNITY_EDITOR
namespace Dunward.Capricorn
{
    [System.Serializable]
    public class PlayMusicUnit : CoroutineUnit
    {
        protected override string info => "Play Music";
    }
}
#endif