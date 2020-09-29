using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace WUG.BehaviorTreeVisualizer
{
    /// <summary>
    /// Does nothing. If there was an animation it would run it
    /// </summary>
    public class Idle : Node
    {
        protected override void OnReset() { }

    protected override NodeStatus OnRun() { return NodeStatus.Success; }

    }
}
