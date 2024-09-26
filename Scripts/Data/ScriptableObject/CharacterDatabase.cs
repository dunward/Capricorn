using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Capricorn/Character Database")]
    public class CharacterDatabase : ScriptableObject
    {
        public List<CharacterTest> characters = new List<CharacterTest>();
    }

    [System.Serializable]
    public class CharacterTest
    {
        public string name;
        public GameObject prefab;
    }
}