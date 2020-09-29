using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUG.BehaviorTreeVisualizer
{
    public class RandomSelector : Selector
    {
        /// <summary>
        /// This selector will shuffle child nodes (once) upon creation
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="nodes">Children nodes to run</param>
        public RandomSelector(string name, params Node[] nodes) : base(name, nodes.OrderBy(g => Guid.NewGuid()).ToArray()) { }

    }
}
