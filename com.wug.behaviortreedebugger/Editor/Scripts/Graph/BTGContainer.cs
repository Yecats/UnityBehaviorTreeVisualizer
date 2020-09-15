using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WUG.BehaviorTreeDebugger
{
    [Serializable]
    public class BTGContainer : ScriptableObject
    {
        public List<BTGNodeLinkData> NodeLinks = new List<BTGNodeLinkData>();
        public List<BTGNodeData> BehaviorTreeNodeData = new List<BTGNodeData>();
    }
}
