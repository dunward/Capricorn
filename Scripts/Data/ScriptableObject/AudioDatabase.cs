using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Capricorn/AudioDatabase", order = 2)]
    public class AudioDatabase : ScriptableObject
    {
        public GameObject audioPrefab;
        public List<AudioTest> audios = new List<AudioTest>();
    }

    [System.Serializable]
    public class AudioTest
    {
        public string name;
        public Sprite sprite;
        [Range(0, 1)]
        public float maxVolume = 1;
    }
}