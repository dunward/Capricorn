namespace Dunward.Capricorn
{
    [System.Serializable]
    public class NodeMainData
    {
        public int id;
        public string title;
        public float x;
        public float y;
        public NodeType nodeType;
        public NodeActionData actionData;
    }

    public enum NodeType
    {
        None,
        Input,
        Output,
        Connector
    }
}