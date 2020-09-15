using System;
using System.Collections.Generic;
using UnityEngine;
using GraphView = UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace WUG.BehaviorTreeDebugger
{
    public enum DecoratorTypes
    {
        None,
        Inverter,
        Timer,
        UntilFail,
        Repeater
    }

    [Serializable]
    public class BTGNodeData : GraphView.Node
    {
        public string Id;
        public Vector2 Position;
        public bool EntryPoint;
        public NodeBase RuntimeNodeRef;
        public SettingsData.NodeProperty Settings;

        public GraphView.Port ParentPort;

        public GraphView.Port InputPort;
        public List<GraphView.Port> OutputPorts = new List<GraphView.Port>();

        private VisualElement m_NodeBorder;
        private VisualElement m_NodeTitleContainer;
        private Label m_NodeTopMessage;
        private Image m_StatusIcon;

        private Color m_defaultBorderColor = new Color(.098f, .098f, .098f);
        private Color m_White = new Color(255, 255, 255);

        public BTGNodeData(NodeBase runtimeNodeRef, bool entryPoint, GraphView.Port parentPort, SettingsData.NodeProperty settings, List<SettingsData.NodeProperty> decoratorData)
        {
            RuntimeNodeRef = runtimeNodeRef;
            RuntimeNodeRef.NodeStatusChanged += OnNodeStatusChanged;

            title = RuntimeNodeRef.Name == null || RuntimeNodeRef.Name.Equals("") ? RuntimeNodeRef.GetType().Name : RuntimeNodeRef.Name;

            Settings = settings;

            Id = Guid.NewGuid().ToString();
            EntryPoint = entryPoint;
            ParentPort = parentPort;

            m_StatusIcon = new Image()
            {
                style =
                {
                    width = 25,
                    height = 25,
                    marginRight = 5,
                    marginTop = 5
                }
            };

            m_StatusIcon.tintColor = m_White;
            titleContainer.Add(m_StatusIcon);

            m_NodeBorder = this.Q<VisualElement>("node-border");
            m_NodeTitleContainer = this.Q<VisualElement>("title");

            m_NodeTitleContainer.style.backgroundColor = new StyleColor(Settings.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.GetDimLevel()));

            m_NodeTopMessage = new Label()
            {
                name = "errorReason",
                style = {
                    color = m_White,
                    backgroundColor = new Color(.17f,.17f,.17f),
                    flexWrap = Wrap.Wrap,
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10,
                    display = DisplayStyle.None,
                    whiteSpace = WhiteSpace.Normal
                }
            };

            //Add the decorator icon

            if (decoratorData != null)
            {
                foreach (var decorator in decoratorData)
                {
                    Image decoratorImage = CreateDecoratorImage(decorator.Icon.texture);

                    m_NodeTitleContainer.Add(decoratorImage);
                    decoratorImage.SendToBack();
                }
            }

            this.Q<VisualElement>("contents").Add(m_NodeTopMessage);
            m_NodeTopMessage.SendToBack();
        }

        private void RuntimeNodeRef_NodeStatusChanged(NodeStatus status, string reason)
        {
            throw new NotImplementedException();
        }

        private Image CreateDecoratorImage(Texture texture)
        {
            Image icon = new Image()
            {
                style =
                {
                    width = 25,
                    height = 25,
                    marginRight = 5,
                    marginTop = 5,
                    marginLeft = 5,
                }
            };

            icon.tintColor = m_White;
            icon.image = texture;

            return icon;
        }


        public void AddPort(GraphView.Port port, string name, bool isInputPort)
        {
            port.portColor = Color.white;
            port.portName = name;

            if (isInputPort)
            {
                inputContainer.Add(port);
                InputPort = port;
            }
            else
            {
                outputContainer.Add(port);
                OutputPorts.Add(port);
            }

            RefreshExpandedState();
            RefreshPorts();
        }

        public void GenerateEdge()
        {
            var tempEdge = new GraphView.Edge()
            {
                output = ParentPort,
                input = InputPort,
            };

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);

            Add(tempEdge);

            RefreshExpandedState();
            RefreshPorts();
        }


        private void OnNodeStatusChanged(NodeStatus status, string reason)
        {
            if (reason == "")
            {
                m_NodeTopMessage.style.display = DisplayStyle.None;
            }
            else
            {
                m_NodeTopMessage.style.display = DisplayStyle.Flex;
                m_NodeTopMessage.text = reason;
            }

            m_StatusIcon.style.visibility = Visibility.Visible;
            ColorPorts(Color.white);
            DefaultBorder();

            switch (status)
            {
                case NodeStatus.Failure:
                    m_StatusIcon.image = Settings.InvertResult ? BehaviorTreeGraphWindow.SettingsData.SuccessIcon.texture : BehaviorTreeGraphWindow.SettingsData.FailureIcon.texture;
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(Settings.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.GetDimLevel()));

                    break;
                case NodeStatus.Success:
                    m_StatusIcon.image = Settings.InvertResult ? BehaviorTreeGraphWindow.SettingsData.FailureIcon.texture : BehaviorTreeGraphWindow.SettingsData.SuccessIcon.texture;
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(Settings.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.GetDimLevel()));

                    break;
                case NodeStatus.Running:
                    m_StatusIcon.image = BehaviorTreeGraphWindow.SettingsData.RunningIcon.texture;
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(Settings.TitleBarColor.WithAlpha(1f));

                    ColorPorts(BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor);
                    RunningBorder();

                    break;
                case NodeStatus.Unknown:
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(Settings.TitleBarColor.WithAlpha(1f));
                    m_StatusIcon.style.visibility = Visibility.Hidden;
                    break;
            }
        }

        private void ColorPorts(Color color)
        {
            if (InputPort != null)
            {
                InputPort.portColor = color;
            }

            if (ParentPort != null)
            {
                ParentPort.portColor = color;
            }
        }

        private void RunningBorder()
        {
            m_NodeBorder.style.borderLeftColor = BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor;
            m_NodeBorder.style.borderRightColor = BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor;
            m_NodeBorder.style.borderTopColor = BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor;
            m_NodeBorder.style.borderBottomColor = BehaviorTreeGraphWindow.SettingsData.BorderHighlightColor;
            m_NodeBorder.style.borderTopWidth = 2f;
            m_NodeBorder.style.borderRightWidth = 2f;
            m_NodeBorder.style.borderLeftWidth = 2f;
            m_NodeBorder.style.borderBottomWidth = 2f;
        }

        private void DefaultBorder()
        {
            m_NodeBorder.style.borderLeftColor = m_defaultBorderColor;
            m_NodeBorder.style.borderRightColor = m_defaultBorderColor;
            m_NodeBorder.style.borderTopColor = m_defaultBorderColor;
            m_NodeBorder.style.borderBottomColor = m_defaultBorderColor;
            m_NodeBorder.style.borderTopWidth = 1f;
            m_NodeBorder.style.borderRightWidth = 1f;
            m_NodeBorder.style.borderLeftWidth = 1f;
            m_NodeBorder.style.borderBottomWidth = 1f;
        }
    }
}
