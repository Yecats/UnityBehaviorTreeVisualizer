namespace WUG.BehaviorTreeDebugger
{

    public class Repeater : Decorator
    {
        private readonly int _repeatCount;

        /// <summary>
        /// Run the child node a specified amount of times before exiting
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="childNode">Node to run</param>
        /// <param name="repeatCount">Amount of times to run</param>
        public Repeater(string name, Node childNode, int repeatCount = 0) : base(name, childNode)
        {
            _repeatCount = repeatCount;
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.Failure;
            }

            //update child
            NodeStatus returnStatus = (ChildNodes[0] as Node).Run();

            //stop if this is a count-limited repeat
            if (_repeatCount > 0 && _repeatCount == EvaluationCount)
            { 
                return NodeStatus.Failure; 
            }

            //otherwise return child state
            if (returnStatus == NodeStatus.Running)
            { 
                return NodeStatus.Success; 
            }

            //finally reset
            Reset();
            (ChildNodes[0] as Node).Reset();
            
            return NodeStatus.Success;
        }
    }
}
