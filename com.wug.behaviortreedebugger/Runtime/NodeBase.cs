using System.Collections.Generic;

namespace WUG.BehaviorTreeDebugger
{

    public delegate void NodeStatusChangedEventHandler(NodeBase sender, NodeStatus status, string reason);
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
        public string m_LastStatusReason { get; set; } = "";
        public List<NodeBase> ChildNodes = new List<NodeBase>();
        public NodeStatus LastNodeStatus = NodeStatus.NotRun;

        public event NodeStatusChangedEventHandler NodeStatusChanged;

        protected virtual void OnNodeStatusChanged(NodeBase sender, NodeStatus status, string reason)
        {
            NodeStatusChanged?.Invoke(sender, status, reason);
        }

    }
}