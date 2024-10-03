using UnityEngine;

using AYellowpaper.SerializedCollections;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Capricorn/Character Database")]
    public class CharacterDatabase : ScriptableObject
    {
        public SerializedDictionary<string, GameObject> characters = new SerializedDictionary<string, GameObject>();
    }
}