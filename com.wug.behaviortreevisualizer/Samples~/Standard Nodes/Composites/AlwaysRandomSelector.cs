namespace WUG.BehaviorTreeVisualizer
{
    public class AlwaysRandomSelector : Selector
    {
        /// <summary>
        /// Shuffles the child nodes every time it runs for the first time 
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="nodes">Children nodes to run</param>
        public AlwaysRandomSelector(string name, params Node[] nodes) : base(name, nodes)
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
