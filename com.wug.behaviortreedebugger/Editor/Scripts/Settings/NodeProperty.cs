using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WUG.BehaviorTreeDebugger;

[Serializable]
public class NodeProperty
{
    public MonoScript Script = null;
    public Color TitleBarColor = SettingsData.DefaultColor;
    public Sprite Icon = null;
    public bool IsDecorator = false;
    public bool InvertResult = false;
}
