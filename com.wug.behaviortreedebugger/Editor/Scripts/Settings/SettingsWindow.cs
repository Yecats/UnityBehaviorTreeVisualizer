using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;
using static WUG.BehaviorTreeDebugger.SettingsData;
using System.IO;

namespace WUG.BehaviorTreeDebugger
{
    public class SettingsWindow : EditorWindow
    {
        private static VisualTreeAsset m_RootTemplate;
        private static Slider m_DimLevel;
        private static ColorField m_ColorField;
        private static Toggle m_MiniMap;
        private static ObjectField m_SuccessIcon;
        private static ObjectField m_RunningIcon;
        private static ObjectField m_FailureIcon;
        private static SettingNodeRow m_BaseNode;
        private static ObjectField m_MainNode;
        private static ObjectField m_OverrideNode;
        private static VisualElement m_MainNodesContainer;
        private static VisualElement m_OverrideNodesContainer;

        private void OnEnable()
        {
            m_RootTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BehaviorTreeGraphWindow.c_WindowPath + "Settings.uxml");
            rootVisualElement.Add(m_RootTemplate.CloneTree());

            m_DimLevel = rootVisualElement.Q<Slider>("Slider_Inactive_Dim");
            m_ColorField = rootVisualElement.Q<ColorField>("clr_BorderHighlight");
            m_MiniMap = rootVisualElement.Q<Toggle>("bool_MiniMap");
            m_SuccessIcon = rootVisualElement.Q<ObjectField>("successIcon");
            m_RunningIcon = rootVisualElement.Q<ObjectField>("runningIcon");
            m_FailureIcon = rootVisualElement.Q<ObjectField>("failureIcon");
            m_MainNode = rootVisualElement.Q<ObjectField>("mainNodeSelector");
            m_OverrideNode = rootVisualElement.Q<ObjectField>("overrideNodeSelector");
            m_BaseNode = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.BaseNodeProperties, SettingNodeType.Base);
            m_MainNodesContainer = rootVisualElement.Q<VisualElement>("mainNodesContainer");
            m_OverrideNodesContainer = rootVisualElement.Q<VisualElement>("overridesContainer");

            rootVisualElement.Q<VisualElement>("baseNodesContainer").Add(m_BaseNode);

            m_MainNode.objectType = typeof(MonoScript);
            m_OverrideNode.objectType = typeof(MonoScript);

            m_MiniMap.RegisterValueChangedCallback((e) => { ToggleMiniMap(e.newValue); });
            m_DimLevel.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.DimLevel = e.newValue; });
            m_ColorField.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor = e.newValue; });
            m_SuccessIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateNodeValue(e.newValue as Sprite, IconType.Success); });
            m_RunningIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateNodeValue(e.newValue as Sprite, IconType.Running); });
            m_FailureIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateNodeValue(e.newValue as Sprite, IconType.Failure); });
            m_OverrideNode.RegisterValueChangedCallback((e) => 
            {
                if (e.newValue != null)
                {
                    SettingNodeRow newRow = new SettingNodeRow(new NodeProperty() { Script = e.newValue as MonoScript }, SettingNodeType.Override);
                    m_OverrideNodesContainer.Add(newRow);
                    newRow.PlaceBehind(m_OverrideNode);

                    m_OverrideNode.value = null;
                }
            });
            m_MainNode.RegisterValueChangedCallback((e) =>
            {
                if (e.newValue != null)
                {
                    SettingNodeRow newRow = new SettingNodeRow(new NodeProperty() { Script = e.newValue as MonoScript }, SettingNodeType.Main);
                    m_MainNodesContainer.Add(newRow);
                    newRow.PlaceBehind(m_MainNode);

                    m_MainNode.value = null;
                }
            });

            DisplaySettings();

        }

        private void OnDestroy()
        {
            SaveSettings();
        }

        /// <summary>
        /// Shows the Node Search modal window
        /// </summary>

        /// <summary>
        /// Writes the display value of the minimap to the settings file and toggles visibility
        /// </summary>
        /// <param name="enabled">Whether the minimap should be visible or not</param>
        public static void ToggleMiniMap(bool enabled)
        {
            BehaviorTreeGraphWindow.SettingsData.EnableMiniMap = enabled;
            BehaviorTreeGraphWindow.Instance.ToggleMinimap(enabled);
        }

        /// <summary>
        /// Load the setting file
        /// </summary>
        public static void LoadSettingsFile()
        {
            SettingsData data = AssetDatabase.LoadAssetAtPath<SettingsData>($"{BehaviorTreeGraphWindow.c_DataPath}/settings.asset");

            if (data == null)
            {
                BehaviorTreeGraphWindow.SettingsData = new SettingsData();
                SaveSettings();
            }
            else
            {
                BehaviorTreeGraphWindow.SettingsData = data;
            }
        }

        /// <summary>
        /// Display the set properties on the settings screen
        /// </summary>
        private static void DisplaySettings()
        {
            m_DimLevel.value = BehaviorTreeGraphWindow.SettingsData.DimLevel;
            m_ColorField.value = BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor;
            m_MiniMap.value = BehaviorTreeGraphWindow.SettingsData.EnableMiniMap;
            m_SuccessIcon.value = BehaviorTreeGraphWindow.SettingsData.SuccessIcon;
            m_FailureIcon.value = BehaviorTreeGraphWindow.SettingsData.FailureIcon;
            m_RunningIcon.value = BehaviorTreeGraphWindow.SettingsData.RunningIcon;

            for (int i = 0; i < BehaviorTreeGraphWindow.SettingsData.OverrideNodeProperties.Count; i++)
            {
                SettingNodeRow newRow = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.OverrideNodeProperties[i], SettingNodeType.Override);

                m_OverrideNodesContainer.Add(newRow);
                newRow.PlaceBehind(m_OverrideNode);
            }

            for (int i = 0; i < BehaviorTreeGraphWindow.SettingsData.MainNodeProperties.Count; i++)
            {
                SettingNodeRow newRow = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.MainNodeProperties[i], SettingNodeType.Main);

                m_MainNodesContainer.Add(newRow);
                newRow.PlaceBehind(m_MainNode);
            }
        }

        /// <summary>
        /// Checks for an existing settings file and if not found, creates one.
        /// </summary>
        private static void SaveSettings()
        {
            if (!AssetDatabase.IsValidFolder(BehaviorTreeGraphWindow.c_RootPathData))
            {
                AssetDatabase.CreateFolder("Assets", "Behavior Tree Debugger (Beta)");
                AssetDatabase.CreateFolder(BehaviorTreeGraphWindow.c_RootPathData, "Resources");
            }

            if (!AssetDatabase.Contains(BehaviorTreeGraphWindow.SettingsData))
            {
                AssetDatabase.CreateAsset(BehaviorTreeGraphWindow.SettingsData, $"{BehaviorTreeGraphWindow.c_DataPath}/settings.asset");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}
