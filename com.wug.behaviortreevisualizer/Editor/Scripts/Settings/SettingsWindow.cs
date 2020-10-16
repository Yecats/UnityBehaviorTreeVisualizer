using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

namespace WUG.BehaviorTreeVisualizer
{
    public class SettingsWindow : EditorWindow
    {
        private static VisualTreeAsset m_RootTemplate;
        private static Slider m_DimLevel;
        private static ColorField m_ColorField;
        private static Toggle m_MiniMap;
        private static Toggle m_LastTimestamp;
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
            m_LastTimestamp = rootVisualElement.Q<Toggle>("bool_LastEvalTimestamp");
            m_SuccessIcon = rootVisualElement.Q<ObjectField>("successIcon");
            m_RunningIcon = rootVisualElement.Q<ObjectField>("runningIcon");
            m_FailureIcon = rootVisualElement.Q<ObjectField>("failureIcon");
            m_MainNode = rootVisualElement.Q<ObjectField>("mainNodeSelector");
            m_OverrideNode = rootVisualElement.Q<ObjectField>("overrideNodeSelector");
            m_BaseNode = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.DataFile.DefaultStyleProperties, SettingNodeType.Base);
            m_MainNodesContainer = rootVisualElement.Q<VisualElement>("mainNodesContainer");
            m_OverrideNodesContainer = rootVisualElement.Q<VisualElement>("overridesContainer");

            rootVisualElement.Q<VisualElement>("baseNodesContainer").Add(m_BaseNode);

            m_MainNode.objectType = typeof(MonoScript);
            m_OverrideNode.objectType = typeof(MonoScript);
            m_SuccessIcon.objectType = typeof(Sprite);
            m_RunningIcon.objectType = typeof(Sprite);
            m_FailureIcon.objectType = typeof(Sprite);

            m_MiniMap.RegisterValueChangedCallback((e) => { ToggleMiniMap(e.newValue); });
            m_LastTimestamp.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.SetLastEvalTimeStamp(e.newValue); });
            m_DimLevel.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.SetDimLevel(e.newValue); });
            m_ColorField.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.SetBorderHighlightColor(e.newValue); });
            m_SuccessIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateGeneralcon(e.newValue as Sprite, IconType.Success); });
            m_RunningIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateGeneralcon(e.newValue as Sprite, IconType.Running); });
            m_FailureIcon.RegisterValueChangedCallback((e) => { BehaviorTreeGraphWindow.SettingsData.UpdateGeneralcon(e.newValue as Sprite, IconType.Failure); });
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

        /// <summary>
        /// Shows the Node Search modal window
        /// </summary>

        /// <summary>
        /// Writes the display value of the minimap to the settings file and toggles visibility
        /// </summary>
        /// <param name="enabled">Whether the minimap should be visible or not</param>
        public static void ToggleMiniMap(bool enabled)
        {
            BehaviorTreeGraphWindow.SettingsData.SetMinimap(enabled);
            BehaviorTreeGraphWindow.Instance.ToggleMinimap(enabled);
        }



        /// <summary>
        /// Display the set properties on the settings screen
        /// </summary>
        private static void DisplaySettings()
        {
            m_DimLevel.value = BehaviorTreeGraphWindow.SettingsData.DataFile.DimLevel;
            m_ColorField.value = BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor;
            m_MiniMap.value = BehaviorTreeGraphWindow.SettingsData.DataFile.EnableMiniMap;
            m_LastTimestamp.value = BehaviorTreeGraphWindow.SettingsData.DataFile.LastRunTimeStamp;
            m_SuccessIcon.value = BehaviorTreeGraphWindow.SettingsData.DataFile.SuccessIcon;
            m_FailureIcon.value = BehaviorTreeGraphWindow.SettingsData.DataFile.FailureIcon;
            m_RunningIcon.value = BehaviorTreeGraphWindow.SettingsData.DataFile.RunningIcon;

            for (int i = 0; i < BehaviorTreeGraphWindow.SettingsData.DataFile.OverrideStyleProperties.Count; i++)
            {
                SettingNodeRow newRow = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.DataFile.OverrideStyleProperties[i], SettingNodeType.Override);

                m_OverrideNodesContainer.Add(newRow);
                newRow.PlaceBehind(m_OverrideNode);
            }

            for (int i = 0; i < BehaviorTreeGraphWindow.SettingsData.DataFile.MainStyleProperties.Count; i++)
            {
                SettingNodeRow newRow = new SettingNodeRow(BehaviorTreeGraphWindow.SettingsData.DataFile.MainStyleProperties[i], SettingNodeType.Main);

                m_MainNodesContainer.Add(newRow);
                newRow.PlaceBehind(m_MainNode);
            }
        }


    }
}
