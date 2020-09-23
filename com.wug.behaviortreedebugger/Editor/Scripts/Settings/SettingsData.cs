using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
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

        public NodeProperty DefaultStyleProperties = new NodeProperty();
        public List<NodeProperty> MainStyleProperties = new List<NodeProperty>();
        public List<NodeProperty> OverrideStyleProperties = new List<NodeProperty>();

    }


}



