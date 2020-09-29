using UnityEngine;
using System.Collections;

namespace WUG.BehaviorTreeVisualizer
{
    public interface IBehaviorTree
    {
        NodeBase BehaviorTree { get; set; }
    }
}