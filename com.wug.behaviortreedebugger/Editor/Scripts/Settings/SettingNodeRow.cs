using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using WUG.BehaviorTreeDebugger;
using static WUG.BehaviorTreeDebugger.SettingsData;

namespace WUG.BehaviorTreeDebugger
{
    public enum SettingNodeType
    {
        Base,
        Main,
        Override
    }

    public class SettingNodeRow : VisualElement
    {
        public static VisualTreeAsset rowTemplate;

        private ObjectField m_NodeScript;
        private ColorField m_NodeColor;
        private ObjectField m_NodeIcon;
        private VisualElement m_ImagePreview;
        private Button m_btnDelete;
        private Toggle m_isDecorator;
        private Toggle m_InvertResult;

        SettingNodeType m_NodeType;
        private NodeProperty m_Settings;
        public NodeProperty Settings
        {
            get => m_Settings;
            set
            {
                switch (m_NodeType)
                {
                    case SettingNodeType.Base:
                        BehaviorTreeGraphWindow.SettingsData.UpdateBaseNodeProperty(value);
                        break;
                    case SettingNodeType.Main:
                        BehaviorTreeGraphWindow.SettingsData.UpdateMainNodeProperty(m_Settings == null ? null : m_Settings.Script, value);
                        break;
                    case SettingNodeType.Override:
                        BehaviorTreeGraphWindow.SettingsData.UpdateOverrideNodeProperty(m_Settings == null ? null : m_Settings.Script, value);
                        break;
                }

                m_Settings = value;
            }
        }

        public SettingNodeRow(NodeProperty settings, SettingNodeType nodeType)
        {
            if (rowTemplate == null)
            {
                rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BehaviorTreeGraphWindow.c_WindowPath + "setting_node_row.uxml");
            }

            Add(rowTemplate.CloneTree());

            m_NodeType = nodeType;
            Settings = settings;

            m_NodeScript = this.Q<ObjectField>("script");
            m_NodeColor = this.Q<ColorField>("color");
            m_NodeIcon = this.Q<ObjectField>("icon");
            m_ImagePreview = this.Q<VisualElement>("imgPreview");
            m_btnDelete = this.Q<Button>("btnDelete");
            m_isDecorator = this.Q<Toggle>("isDecorator");
            m_InvertResult = this.Q<Toggle>("invertResults");

            if (nodeType == SettingNodeType.Base)
            {
                m_NodeScript.parent.Remove(m_NodeScript);
                m_btnDelete.parent.Remove(m_btnDelete);
                m_isDecorator.parent.Remove(m_isDecorator);
                m_InvertResult.parent.Remove(m_InvertResult);

                m_NodeColor.style.marginLeft = 2;
            }
            else
            {
                //Set existing values
                m_isDecorator.value = Settings.IsDecorator;
                m_InvertResult.value = Settings.InvertResult;
                m_NodeScript.value = Settings.Script;

                //Register callbacks
                m_btnDelete.clicked += Delete_OnClick;
                m_isDecorator.RegisterValueChangedCallback((e) =>
                {
                    NodeProperty original = Settings;
                    original.IsDecorator = e.newValue;
                    Settings = original;

                });
                m_InvertResult.RegisterValueChangedCallback((e) =>
                {
                    NodeProperty original = Settings;
                    original.InvertResult = e.newValue;
                    Settings = original;

                });


                //Register callbacks
                m_NodeScript.RegisterValueChangedCallback((e) =>
                {
                    NodeProperty original = Settings;
                    original.Script = e.newValue as MonoScript;
                    Settings = original;
                });
            }

            //Set existing values
            m_NodeColor.value = Settings.TitleBarColor;

            if (Settings.Icon != null)
            {
                m_NodeIcon.value = Settings.Icon;
                m_ImagePreview.style.backgroundImage = new StyleBackground(Settings.Icon.texture);
            }

            m_NodeColor.RegisterValueChangedCallback((e) =>
            {
                NodeProperty original = Settings;
                original.TitleBarColor = e.newValue;
                Settings = original;
            });

            m_NodeIcon.RegisterValueChangedCallback((e) =>
            {
                Sprite newIcon = e.newValue as Sprite;
                NodeProperty original = Settings;

                original.Icon = newIcon;
                Settings = original;

                m_ImagePreview.style.backgroundImage = new StyleBackground(newIcon.texture);
            });
        }

        public void Delete_OnClick()
        {
            switch (m_NodeType)
            {
                case SettingNodeType.Main:
                    BehaviorTreeGraphWindow.SettingsData.RemoveMainNode(Settings.Script);
                    break;
                case SettingNodeType.Override:
                    BehaviorTreeGraphWindow.SettingsData.RemoveOverrideNode(Settings.Script);
                    break;
            }

            this.parent.Remove(this);
        }

    }
}