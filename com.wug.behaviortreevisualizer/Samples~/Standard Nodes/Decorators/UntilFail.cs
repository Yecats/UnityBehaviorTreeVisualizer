

namespace WUG.BehaviorTreeVisualizer
{
    public class UntilFail : Decorator
    {
        /// <summary>
        /// Continues to run the child node unless the child returns a status of Failure.
        /// </summary>
        /// <param name="nodeName">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="childNode">Child to run</param>
        public UntilFail(string nodeName, Node childNode) : base(nodeName, childNode) { }
        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            return (ChildNodes[0] as Node).Run() == NodeStatus.Failure ? NodeStatus.Failure : NodeStatus.Running;

        }
    }
}
