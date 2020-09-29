using System;
using UnityEditor;
using UnityEngine;

namespace WUG.BehaviorTreeVisualizer
{
    [Serializable]
    public class NodeProperty
    {
        public MonoScript Script = null;
        public Color TitleBarColor = SettingsData.DefaultColor;
        public Sprite Icon = null;
        public bool IsDecorator = false;
        public bool InvertResult = false;
    }
}
