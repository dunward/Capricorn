using System.Collections.Generic;
using UnityEngine;

namespace Dunward.Capricorn
{
    [CreateAssetMenu(fileName = "BackgroundDatabase", menuName = "Capricorn/BackgroundDatabase", order = 1)]
    public class BackgroundDatabase : ScriptableObject
    {
        public GameObject backgroundPrefab;
        public List<BackgroundTest> backgrounds = new List<BackgroundTest>();
    }

    [System.Serializable]
    public class BackgroundTest
    {
        public string name;
        public Sprite sprite;
    }
}