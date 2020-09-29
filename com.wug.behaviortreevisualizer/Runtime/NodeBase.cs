using System.Collections.Generic;

namespace WUG.BehaviorTreeVisualizer
{

    public delegate void NodeStatusChangedEventHandler(NodeBase sender);
    public enum NodeStatus
    {
        Failure,
        Success,
        Running, //evaluation incomplete
        Unknown,
        NotRun
    }
    public class NodeBase
    {
        public string Name { get; set; }
        public string StatusReason { get; set; } = "";
        public List<NodeBase> ChildNodes = new List<NodeBase>();
        public NodeStatus LastNodeStatus = NodeStatus.NotRun;
        
        public event NodeStatusChangedEventHandler NodeStatusChanged;

        /// <summary>
        /// Handles invoking the NodeStatusChangedEventHandler delegate.
        /// </summary>
        protected virtual void OnNodeStatusChanged(NodeBase sender)
        {
            NodeStatusChanged?.Invoke(sender);
        }

    }
}