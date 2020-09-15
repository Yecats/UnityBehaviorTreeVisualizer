namespace WUG.BehaviorTreeDebugger
{
    public class Sequence : Composite
    {
        private int _currentChildIndex = 0;

        /// <summary>
        /// A sequence will continue to run as long as all children return the status of Running or Success. 
        /// It will return a status code of Failure if any children return status Failure.
        /// It will only return status of Success once all children have returned a status of Success.
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="nodes">Children node(s) to run</param>
        public Sequence(string name, params Node[] nodes) : base(name, nodes) { }

        protected override void OnReset()
        {
            _currentChildIndex = 0;

            for (int i = 0; i < ChildNodes.Count; i++)
            {
                (ChildNodes[i] as Node).Reset();
            }
        }

        protected override NodeStatus OnRun()
        {
            //Check the status of the last child
            NodeStatus childNodeStatus = (ChildNodes[_currentChildIndex] as Node).Run();

            //Evaluate the current child node. If it's failed - sequence should fail. 
            switch (childNodeStatus)
            {
                //Child failed - return failure
                case NodeStatus.Failure: 
                    return childNodeStatus;
                //It succeeded - move to the next child
                case NodeStatus.Success: 
                    _currentChildIndex++;
                    break;
            }

            //All children have run successfully - return success
            if (_currentChildIndex >= ChildNodes.Count)
            {
                return NodeStatus.Success;
            }

            //The child was a success but we still have more to do - so call this method again.
            return childNodeStatus == NodeStatus.Success ? OnRun() : NodeStatus.Running;
        }

    }
}
