using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUG.BehaviorTreeVisualizer
{
    public class AlwaysRandomSequence : Sequence
    {

        /// <summary>
        /// Shuffles the child nodes every time it runs for the first time 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodes"></param>
        public AlwaysRandomSequence(string name, params Node[] nodes) : base(name, nodes)
        {
            ShuffleNodes();
        }

        /// <summary>
        /// Shuffle nodes after reset
        /// </summary>
        protected override void OnReset()
        {
            base.OnReset();
            ShuffleNodes();
        }

    }
}
