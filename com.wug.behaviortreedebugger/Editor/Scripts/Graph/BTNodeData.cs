using System;
using System.Collections.Generic;
using UnityEngine;
using GraphView = UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using static WUG.BehaviorTreeDebugger.BehaviorTreeGraphView;

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
        public FullNodeInfo MainNodeDetails;
        private List<FullNodeInfo> m_DecoratorData;

        public GraphView.Port ParentPort;

        public GraphView.Port InputPort;
        public List<GraphView.Port> OutputPorts = new List<GraphView.Port>();

        private VisualElement m_NodeBorder;
        private VisualElement m_NodeTitleContainer;
        private Label m_NodeTopMessageGeneral;
        private Label m_NodeTopMessageDecorator;
        private Image m_StatusIcon;

        private Color m_defaultBorderColor = new Color(.098f, .098f, .098f);
        private Color m_White = new Color(255, 255, 255);

        public BTGNodeData(FullNodeInfo mainNodeDetails, bool entryPoint, GraphView.Port parentPort, List<FullNodeInfo> decoratorData)
        {
            MainNodeDetails = mainNodeDetails;
            MainNodeDetails.RunTimeNode.NodeStatusChanged += OnNodeStatusChanged;
            m_DecoratorData = decoratorData;

            title = MainNodeDetails.RunTimeNode.Name == null || MainNodeDetails.RunTimeNode.Name.Equals("") ? MainNodeDetails.RunTimeNode.GetType().Name : MainNodeDetails.RunTimeNode.Name;

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

            m_NodeTitleContainer.style.backgroundColor = new StyleColor(MainNodeDetails.PropertyData.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.DataFile.DimLevel));

            m_NodeTopMessageGeneral = GenerateStatusMessageLabel();
            m_NodeTopMessageDecorator = GenerateStatusMessageLabel();

            //Add the decorator icon
            if (m_DecoratorData != null)
            {
                foreach (var decorator in m_DecoratorData)
                {
                    decorator.RunTimeNode.NodeStatusChanged += OnNodeStatusChanged;

                    Image decoratorImage = CreateDecoratorImage(decorator.PropertyData.Icon.texture);

                    m_NodeTitleContainer.Add(decoratorImage);
                    decoratorImage.SendToBack();

                }
            }

            this.Q<VisualElement>("contents").Add(m_NodeTopMessageGeneral);
            this.Q<VisualElement>("contents").Add(m_NodeTopMessageDecorator);
            m_NodeTopMessageGeneral.SendToBack();
            m_NodeTopMessageDecorator.SendToBack();

            //Do an initial call to setup the style of the node in the event that it's already been running (pretty likely)
            OnNodeStatusChanged(MainNodeDetails.RunTimeNode, MainNodeDetails.RunTimeNode.LastNodeStatus, MainNodeDetails.RunTimeNode.StatusReason);

            if (m_DecoratorData != null)
            {
                m_DecoratorData.ForEach(x => OnNodeStatusChanged(x.RunTimeNode, x.RunTimeNode.LastNodeStatus, x.RunTimeNode.StatusReason));
            }
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

        /// <summary>
        /// Generates a standard label used for messages
        /// </summary>
        /// <returns>Label with the proper style set</returns>
        private Label GenerateStatusMessageLabel()
        {
           return new Label()
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
        }

        /// <summary>
        /// Adds a port to the graph node
        /// </summary>
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

        /// <summary>
        /// Generates a connection (edge) between graph nodes
        /// </summary>
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

        private void OnNodeStatusChanged(NodeBase sender, NodeStatus status, string reason)
        {
            if (MainNodeDetails.RunTimeNode != sender)
            {
                if (reason == "")
                {
                    m_NodeTopMessageDecorator.style.display = DisplayStyle.None;
                }
                else
                {
                    m_NodeTopMessageDecorator.style.display = DisplayStyle.Flex;
                    m_NodeTopMessageDecorator.text = reason;
                }
            }
            else
            {
                if (reason == "")
                {
                    m_NodeTopMessageGeneral.style.display = DisplayStyle.None;
                }
                else
                {
                    m_NodeTopMessageGeneral.style.display = DisplayStyle.Flex;
                    m_NodeTopMessageGeneral.text = reason;
                }

            }

            m_StatusIcon.style.visibility = Visibility.Visible;
            ColorPorts(Color.white);
            DefaultBorder();

            switch (status)
            {
                case NodeStatus.Failure:
                    if (BehaviorTreeGraphWindow.SettingsData.DataFile.FailureIcon != null && BehaviorTreeGraphWindow.SettingsData.DataFile.SuccessIcon != null)
                    {
                        UpdateStatusIcon(MainNodeDetails.PropertyData.InvertResult ? BehaviorTreeGraphWindow.SettingsData.DataFile.SuccessIcon.texture : BehaviorTreeGraphWindow.SettingsData.DataFile.FailureIcon.texture);
                    }
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(MainNodeDetails.PropertyData.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.DataFile.DimLevel));

                    break;
                case NodeStatus.Success:
                    if (BehaviorTreeGraphWindow.SettingsData.DataFile.FailureIcon != null && BehaviorTreeGraphWindow.SettingsData.DataFile.SuccessIcon != null)
                    {
                        UpdateStatusIcon(MainNodeDetails.PropertyData.InvertResult ? BehaviorTreeGraphWindow.SettingsData.DataFile.FailureIcon.texture : BehaviorTreeGraphWindow.SettingsData.DataFile.SuccessIcon.texture);
                    }
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(MainNodeDetails.PropertyData.TitleBarColor.WithAlpha(BehaviorTreeGraphWindow.SettingsData.DataFile.DimLevel));

                    break;
                case NodeStatus.Running:
                    if (BehaviorTreeGraphWindow.SettingsData.DataFile.RunningIcon != null)
                    {
                        UpdateStatusIcon(BehaviorTreeGraphWindow.SettingsData.DataFile.RunningIcon.texture);
                    }
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(MainNodeDetails.PropertyData.TitleBarColor.WithAlpha(1f));

                    ColorPorts(BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor);
                    RunningBorder();

                    break;
                case NodeStatus.Unknown:
                    m_NodeTitleContainer.style.backgroundColor = new StyleColor(MainNodeDetails.PropertyData.TitleBarColor.WithAlpha(1f));
                    m_StatusIcon.style.visibility = Visibility.Hidden;
                    break;
            }
        }

        private void UpdateStatusIcon(Texture newImage)
        {
            if (newImage == null)
            {
                return;
            }

            m_StatusIcon.image = newImage;
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
            m_NodeBorder.style.borderLeftColor = BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor;
            m_NodeBorder.style.borderRightColor = BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor;
            m_NodeBorder.style.borderTopColor = BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor;
            m_NodeBorder.style.borderBottomColor = BehaviorTreeGraphWindow.SettingsData.DataFile.BorderHighlightColor;
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
