namespace Dunward.Capricorn
{
    public class ActionPlayer
    {
        protected NodeActionData actionData;

        protected bool isComplete = false;

        public ActionPlayer(NodeActionData actionData)
        {
            this.actionData = actionData;
        }
        
        public virtual int Next()
        {
            return actionData.connections[0];
        }
    }
}