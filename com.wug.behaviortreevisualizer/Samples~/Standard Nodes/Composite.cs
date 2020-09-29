using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUG.BehaviorTreeVisualizer
{
    public abstract class Composite : Node
    {
        //constructor
        protected Composite(string name, params Node[] nodes)
        {
            Name = name;

            ChildNodes.AddRange(nodes.ToList());
        }

        protected void ShuffleNodes() => ChildNodes = ChildNodes.OrderBy(g => Guid.NewGuid()).ToList();

        protected void WeightedShuffleNodes(List<(int, Node)> nodeWeights)
        {
            //get node list copy
            List<(int, Node)> availableNodes = new List<(int, Node)>(nodeWeights.OrderBy(g => Guid.NewGuid()));

            //new child list
            List<NodeBase> newChildList = new List<NodeBase>();

            //build new list
            while (availableNodes.Count > 0)
            {
                foreach ((int, Node) node in availableNodes)
                {
                    //check if selection
                    if (UnityEngine.Random.Range(0, availableNodes.Sum(w => w.Item1)) < node.Item1)
                    {
                        //add child and remove from pool
                        newChildList.Add(node.Item2);
                        availableNodes.Remove(node);
                        break;
                    }
                }
            }

            //update list
            ChildNodes = newChildList.ToList();
        }

    }
}
