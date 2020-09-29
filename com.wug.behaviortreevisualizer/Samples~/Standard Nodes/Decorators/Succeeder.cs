using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUG.BehaviorTreeVisualizer
{
    public class Succeeder : Decorator
    {
        /// <summary>
        /// Returns Running status if running otherwise Success - Never failure.
        /// </summary>
        /// <param name="nodeName">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="childNode">Child to run</param>
        public Succeeder(string nodeName, Node childNode) : base(nodeName, childNode) { }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            return (ChildNodes[0] as Node).Run() == NodeStatus.Running ? NodeStatus.Running : NodeStatus.Success;
        }
    }
}
