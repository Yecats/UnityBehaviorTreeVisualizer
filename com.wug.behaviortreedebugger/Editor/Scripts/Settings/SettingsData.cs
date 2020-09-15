using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WUG.BehaviorTreeDebugger
{
    public enum IconType
    {
        Running,
        Success,
        Failure
    }

    public class SettingsData : ScriptableObject
    {
        public static Color DefaultColor = new Color().FromRGB(24, 181, 233);

        public float DimLevel = 127f;
        public Color BorderHighlightColor = Color.green;
        public bool EnableMiniMap = false;
        public Sprite RunningIcon = null;
        public Sprite SuccessIcon = null;
        public Sprite FailureIcon = null;

        public NodeProperty BaseNodeProperties = null;
        public List<NodeProperty> MainNodeProperties = new List<NodeProperty>();
        public List<NodeProperty> OverrideNodeProperties = new List<NodeProperty>();

        [Serializable]
        public class NodeProperty
        {
            public MonoScript Script = null;
            public Color TitleBarColor = DefaultColor;
            public Sprite Icon = null;
            public bool IsDecorator = false;
            public bool InvertResult = false;
        }

        public void UpdateBaseNodeProperty(NodeProperty newProperties)
        {
            BaseNodeProperties.TitleBarColor = newProperties.TitleBarColor;
            BaseNodeProperties.Icon = newProperties.Icon;
        } 

        public void UpdateMainNodeProperty(MonoScript originalScript, NodeProperty newProperties)
        {
            NodeProperty node = originalScript == null ? MainNodeProperties.FirstOrDefault(x => x.Script == newProperties.Script) : 
                MainNodeProperties.FirstOrDefault(x => x.Script == originalScript);

            if (node != null)
            {
                node = newProperties;
            }
            else
            {
                MainNodeProperties.Add(newProperties);
            }
        }

        public void UpdateOverrideNodeProperty(MonoScript originalScript, NodeProperty newProperties)
        {
            NodeProperty node = originalScript == null ? OverrideNodeProperties.FirstOrDefault(x => x.Script == newProperties.Script) :
                OverrideNodeProperties.FirstOrDefault(x => x.Script == originalScript);

            if (node != null)
            {
                node = newProperties;
            }
            else
            {
                AddOverrideNode(newProperties);
            }
        }

        public void AddMainNode(NodeProperty newNode)
        {
            MainNodeProperties.Add(newNode);
        }

        public void AddOverrideNode(NodeProperty newNode)
        {
            OverrideNodeProperties.Add(newNode);
        }

        public void RemoveOverrideNode(MonoScript script)
        {
            NodeProperty node = OverrideNodeProperties.FirstOrDefault(x => x.Script == script);

            if (node != null)
            {
                  OverrideNodeProperties.Remove(node);
            }
          
        }

        public void RemoveMainNode(MonoScript script)
        {
            NodeProperty node = MainNodeProperties.FirstOrDefault(x => x.Script == script);

            if (node != null)
            {
                MainNodeProperties.Remove(node);
            }
        }

        public NodeProperty GetNodePropertyDetails(object script)
        {

            NodeProperty node = OverrideNodeProperties.FirstOrDefault(x => script.GetType().Equals(x.Script.GetClass()));

            if (node == null)
            {
                node = MainNodeProperties.FirstOrDefault(x => script.GetType().IsSubclassOf(x.Script.GetClass()) || script.GetType().Equals(x.Script.GetClass()));
            }

            if (node == null)
            {
                node = BaseNodeProperties;
            }

            return node;
        }

        public void UpdateNodeValue(Sprite icon, IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Running:
                    RunningIcon = icon;
                    break;
                case IconType.Success:
                    SuccessIcon = icon;
                    break;
                case IconType.Failure:
                    FailureIcon = icon;
                    break;
            }
        }

        public float GetDimLevel()
        {
            return DimLevel / 255;
        }
    }
}



