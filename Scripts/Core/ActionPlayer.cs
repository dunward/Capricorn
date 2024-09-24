using System.Collections;

namespace Dunward.Capricorn
{
    public class ActionPlayer
    {
        protected NodeActionData actionData;

        public ActionPlayer(NodeActionData actionData)
        {
            this.actionData = actionData;
        }

        public virtual IEnumerator Run()
        {
            yield return null;
        }

        public virtual int Next()
        {
            return actionData.connections[0];
        }
    }
}