namespace Dunward.Capricorn
{
    [System.Serializable]
    public class NodeMainData
    {
        public int id;
        public float x;
        public float y;
        public NodeActionData actionData;
        public NodeType nodeType;
    }

    public enum NodeType
    {
        None,
        Input,
        Output,
        Connector
    }
}