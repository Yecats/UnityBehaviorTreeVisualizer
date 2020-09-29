using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using static WUG.BehaviorTreeVisualizer.SettingsData;

public class DataManager
{
    public SettingsData DataFile { get; private set; }
    private SerializedObject m_SettingsRef;

    public DataManager()
    {
        LoadSettingsFile();

        m_SettingsRef = new SerializedObject(DataFile);
    }

    /// <summary>
    /// Set properties related to the default style
    /// </summary>
    public void SetDefaultStyle(NodeProperty newProperties)
    {

        if (m_SettingsRef == null)
        {
            "Settings were not serialized properly and cannot be loaded.".BTDebugLog();
            return;
        }

        m_SettingsRef.FindProperty("DefaultStyleProperties").FindPropertyRelative("TitleBarColor").colorValue = newProperties.TitleBarColor;
        m_SettingsRef.FindProperty("DefaultStyleProperties").FindPropertyRelative("Icon").objectReferenceValue = newProperties.Icon;

        SaveSettingsFile();
    }

    /// <summary>
    /// Set properties for either a main or override style
    /// </summary>
    /// <param name="propertyName">The name of the list to adjust - should be either "MainStyleProperties" or "OverrideStyleProperties"</param>
    /// <param name="newValue">New style data to save</param>
    /// <param name="originalScript">Script previously stored if style is being updated</param>
    public void SetMainOrOverrideStyle(string propertyName, NodeProperty newValue, MonoScript originalScript = null)
    {
        if (m_SettingsRef == null)
        {
            "Settings were not serialized properly and cannot be loaded.".BTDebugLog();
            return;
        }

        var property = m_SettingsRef.FindProperty(propertyName);
        int index = property.arraySize;

        //Look for an existing script reference
        for (int i = 0; i < property.arraySize; i++)
        {
            MonoScript script = property.GetArrayElementAtIndex(i).FindPropertyRelative("Script").objectReferenceValue as MonoScript;

            if (script == originalScript || script == newValue.Script)
            {
                index = i;
                break;
            }
        }

        //Need to add a new empty element to update because nothing was found
        if (index == property.arraySize)
        {
            property.InsertArrayElementAtIndex(index);
        }

        //Update each of the properties

        property.GetArrayElementAtIndex(index).FindPropertyRelative("Script").objectReferenceValue = newValue.Script;
        property.GetArrayElementAtIndex(index).FindPropertyRelative("TitleBarColor").colorValue = newValue.TitleBarColor;
        property.GetArrayElementAtIndex(index).FindPropertyRelative("Icon").objectReferenceValue = newValue.Icon;
        property.GetArrayElementAtIndex(index).FindPropertyRelative("IsDecorator").boolValue = newValue.IsDecorator;
        property.GetArrayElementAtIndex(index).FindPropertyRelative("InvertResult").boolValue = newValue.InvertResult;

        SaveSettingsFile();
    }

    /// <summary>
    /// Set the border highlight color
    /// </summary>
    internal void SetBorderHighlightColor(Color newValue)
    {
        m_SettingsRef.FindProperty("BorderHighlightColor").colorValue = newValue;
        SaveSettingsFile();
    }

    /// <summary>
    /// Set the dim level of inactive drawn nodes
    /// </summary>
    public void SetDimLevel(float newValue)
    {
        m_SettingsRef.FindProperty("DimLevel").floatValue = newValue;

        SaveSettingsFile();
    }

    /// <summary>
    /// Set whether the minimap is visible
    /// </summary>
    public void SetMinimap(bool newValue)
    {
        m_SettingsRef.FindProperty("EnableMiniMap").boolValue = newValue;
        SaveSettingsFile();
    }
    
    /// <summary>
    /// Set the running, success or failure icons
    /// </summary>
    public void UpdateGeneralcon(Sprite newIcon, IconType iconType)
    {
        if (m_SettingsRef == null)
        {
            "Settings were not serialized properly and cannot be loaded.".BTDebugLog();
            return;
        }

        switch (iconType)
        {
            case IconType.Running:
                m_SettingsRef.FindProperty("RunningIcon").objectReferenceValue = newIcon;
                break;
            case IconType.Success:
                m_SettingsRef.FindProperty("SuccessIcon").objectReferenceValue = newIcon;
                break;
            case IconType.Failure:
                m_SettingsRef.FindProperty("FailureIcon").objectReferenceValue = newIcon;
                break;
        }

        SaveSettingsFile();
    }

    /// <summary>
    /// Remove an override or main style from the list of styles
    /// </summary>
    /// <param name="propertyName">The name of the list to adjust - should be either "MainStyleProperties" or "OverrideStyleProperties"</param>
    /// <param name="existingScript">Script that the style was applied to</param>
    public void RemoveMainOrOverrideStyle(string propertyName, MonoScript existingScript)
    {
        if (m_SettingsRef == null)
        {
            "Settings were not serialized properly and cannot be loaded.".BTDebugLog();
            return;
        }

        var property = m_SettingsRef.FindProperty(propertyName);
        int index = -1;

        //Look for an existing script reference
        for (int i = 0; i < property.arraySize; i++)
        {
            MonoScript script = property.GetArrayElementAtIndex(i).FindPropertyRelative("Script").objectReferenceValue as MonoScript;

            if (script == existingScript)
            {
                index = i;
                break;
            }
        }

        //If this is greater than -1, something was found
        if (index >= 0)
        {
            property.DeleteArrayElementAtIndex(index);
        }

        SaveSettingsFile();

    }

    /// <summary>
    /// Get the style information for a specific script
    /// </summary>
    public NodeProperty GetNodeStyleDetails(object scriptToFind)
    {
        NodeProperty node = DataFile.OverrideStyleProperties.FirstOrDefault(x => scriptToFind.GetType().Equals(x.Script.GetClass()));

        if (node == null)
        {
            node = DataFile.MainStyleProperties.FirstOrDefault(x => scriptToFind.GetType().IsSubclassOf(x.Script.GetClass()) || scriptToFind.GetType().Equals(x.Script.GetClass()));
        }

        if (node == null)
        {
            node = DataFile.DefaultStyleProperties;
        }

        return node;
    }

    public float GetDimLevel()
    {
        return DataFile.DimLevel / 255;
    }

    /// <summary>
    /// Save the settings scriptable object file
    /// </summary>
    private void SaveSettingsFile()
    {
        EditorUtility.SetDirty(DataFile);
        m_SettingsRef.ApplyModifiedProperties();
    }

    /// <summary>
    /// Load the setting file
    /// </summary>
    public void LoadSettingsFile()
    {
        SettingsData data = AssetDatabase.LoadAssetAtPath<SettingsData>($"{BehaviorTreeGraphWindow.c_DataPath}/settings.asset");

        if (data == null)
        {
            DataFile = ScriptableObject.CreateInstance("SettingsData") as SettingsData;
            CreateSettingsFile();
        }
        else
        {
            DataFile = data;
        }
    }

    /// <summary>
    /// Checks for an existing settings file and if not found, creates one.
    /// </summary>
    private void CreateSettingsFile()
    {
        if (!AssetDatabase.IsValidFolder(BehaviorTreeGraphWindow.c_RootPathData))
        {
            AssetDatabase.CreateFolder("Assets", "Behavior Tree Visualizer (Beta)");
            AssetDatabase.CreateFolder(BehaviorTreeGraphWindow.c_RootPathData, "Resources");
        }

        if (!AssetDatabase.Contains(DataFile))
        {
            AssetDatabase.CreateAsset(DataFile, $"{BehaviorTreeGraphWindow.c_DataPath}/settings.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
