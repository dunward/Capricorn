using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Capricorn/AudioDatabase", order = 2)]
    public class AudioDatabase : ScriptableObject
    {
        public GameObject bgmPrefab;
        public GameObject sfxPrefab;
        public List<AudioTest> bgms = new List<AudioTest>();
        public List<AudioTest> sfxs = new List<AudioTest>();
    }

    [System.Serializable]
    public class AudioTest
    {
        public string name;
        public AudioClip clip;
        [Range(0, 1)]
        public float maxVolume = 1;
    }
}