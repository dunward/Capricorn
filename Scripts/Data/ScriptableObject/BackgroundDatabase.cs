using UnityEngine;

using AYellowpaper.SerializedCollections;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "BackgroundDatabase", menuName = "Capricorn/BackgroundDatabase", order = 1)]
    public class BackgroundDatabase : ScriptableObject
    {
        public GameObject backgroundPrefab;
        public SerializedDictionary<string, Sprite> backgrounds = new SerializedDictionary<string, Sprite>();
    }

    [System.Serializable]
    public class BackgroundTest
    {
        public string name;
        public Sprite sprite;
    }
}