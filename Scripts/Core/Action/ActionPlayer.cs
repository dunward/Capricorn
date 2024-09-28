namespace Dunward.Capricorn
{
    public class ActionPlayer
    {
        protected NodeActionData actionData;

        protected bool isComplete = false;
        protected int nextConnection = 0;

        public ActionPlayer(NodeActionData actionData)
        {
            this.actionData = actionData;
        }

        public virtual int GetNextNodeIndex()
        {
            if (actionData.connections.Count == 0) return -1; // Output node error handling - TODO: clean up action player class
            return actionData.connections[nextConnection];
        }
    }
}