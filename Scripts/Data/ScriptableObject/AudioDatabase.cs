using System.Collections.Generic;

using UnityEngine;

using AYellowpaper.SerializedCollections;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Capricorn/AudioDatabase", order = 2)]
    public class AudioDatabase : ScriptableObject
    {
        public GameObject bgmPrefab;
        public GameObject sfxPrefab;
        public SerializedDictionary<string, AudioData> bgms = new SerializedDictionary<string, AudioData>();
        public SerializedDictionary<string, AudioData> sfxs = new SerializedDictionary<string, AudioData>();
    }

    [System.Serializable]
    public class AudioData
    {
        public AudioClip clip;
        [Range(0, 1)]
        public float maxVolume = 1;
    }
}