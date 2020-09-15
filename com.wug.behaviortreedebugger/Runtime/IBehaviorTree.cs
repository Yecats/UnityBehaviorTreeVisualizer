using UnityEngine;
using System.Collections;

namespace WUG.BehaviorTreeDebugger
{
    public interface IBehaviorTree
    {
        NodeBase BehaviorTree { get; set; }
    }
}