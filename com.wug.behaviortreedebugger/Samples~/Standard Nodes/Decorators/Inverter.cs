using UnityEngine;

namespace WUG.BehaviorTreeDebugger
{
    public class Inverter : Decorator
    {
        /// <summary>
        /// Inverts the final result of the node - Success will return failure and failure will return success.
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="childNode">Node to alter</param>
        public Inverter(string name, Node childNode) : base(name, childNode) { }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {

            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.Failure;
            }

            NodeStatus originalStatus = (ChildNodes[0] as Node).Run();

            switch (originalStatus)
            {
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                case NodeStatus.Success:
                    return NodeStatus.Failure;
            }

            "Inverter decorator has failed".BTDebugLog();
            return originalStatus;

        }
    }
}
