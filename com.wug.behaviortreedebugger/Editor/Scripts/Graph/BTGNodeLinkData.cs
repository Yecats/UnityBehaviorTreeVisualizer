using System;

namespace WUG.BehaviorTreeDebugger
{
    [Serializable]
    public class BTGNodeLinkData
    {
        public string BaseNodeId;
        public string PortName;
        public string TargetNodeId;
    }
}
