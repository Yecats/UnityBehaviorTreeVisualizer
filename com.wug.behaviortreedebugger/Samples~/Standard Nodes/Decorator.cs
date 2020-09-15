namespace WUG.BehaviorTreeDebugger
{
    public abstract class Decorator : Node
    {

        public Decorator(string name, Node node)
        {
            Name = name;
            ChildNodes.Add(node);
        }

    }
}
