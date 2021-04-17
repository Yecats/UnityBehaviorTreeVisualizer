![beta](https://img.shields.io/badge/release-beta-orange) ![GitHub issues](https://img.shields.io/github/issues/Yecats/UnityBehaviorTreeDebugger) ![version](https://img.shields.io/badge/unity%20version-2019.4%2B-blue) 

![twitter](https://img.shields.io/twitter/follow/yecats131?style=social) ![Twitter](https://img.shields.io/twitter/follow/whatupgames?style=social)

> Want to learn the fundamentals of behavior trees? Check out my tutorial, [Get Started With Behavior Trees in Unity](https://gamedev-resources.com/ai/301/).

# Unity Behavior Tree Visualizer (Beta)
Behavior Trees are a fantastic way to write modular AI that can scale in complexity. Unfortunately, it can be quite hard to visualize how your tree is being executed which makes it difficult to debug potential failure points. The Behavior Tree Visualizer tool was created to solve these problems! The tool will scan for active behavior trees in your scene and group them in a drop down for easy toggle. A graph will be drawn, and nodes will light up, showing you which part of the tree is currently running. 

> Head over to the [Wiki](https://github.com/Yecats/UnityBehaviorTreeDebugger/wiki) for more detailed documentation.

![Example](https://github.com/Yecats/UnityBehaviorTreeVisualizer/blob/main/Wiki/Images/treeExample.gif)


## Features
1. **Customize the graph** by choosing the title bar color, the icon, amount to dim inactive nodes and more.
2. **Robust debug messages** can be viewed directly on the graph. Surface anything you want to see.
3. **Includes basic node types** to help you get up and running quickly. No need to write a sequencer, selector, inverter, or more!

## What's Included?
This package comes with:

1. Behavior Tree Visualizer tool built with Unity Toolkit (formerly UI Elements)
2. Standard Behavior Tree nodes to get you up and running quickly
3. Sample project to demonstrate the implementation

## Setup
To import the package into your project, you can either download the .zip from the **Releases** tab or [install through Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with this URL: 
https://github.com/Yecats/UnityBehaviorTreeDebugger.git?path=/com.wug.behaviortreevisualizer

Here are the most important things to know:

1. Your base Node must inherit from `NodeBase`.
2. Your node should call the `OnNodeStatusChanged` method when the node's status code or reason has been changed. This is what the tool listens to, to know which nodes should be highlighted, what their status is, and whether to draw any debug messages.
3. By inheriting `NodeBase` all nodes will have the notion of child node(s). Do not add anything to the list, and it'll be ignored (and thus, treated as a leaf node). Decorators (inverter, untilfail, etc.) should only ever have one child and composites (seqencer, selector, etc.) can have as many as they need.
4. Your MonoBehavior that runs the behavior tree must implement 'IBehaviorTree'. The tool scans the running scene for all references to this interface.

### Example - Call OnNodeStatusChanged only when a new status or message occurs
1. Your base class must inherit from the `NodeBase` class.
2. Call `OnNodeStatusChanged` event when the node's status has been changed. In my example, I call it only if the all up status has changed (i.e. `Running` to `Success`) or if the `StatusReason` has changed. Here's an example:

``` csharp
private string m_LastStatusReason;


if (LastNodeStatus != nodeStatus || !m_LastStatusReason.Equals(StatusReason))
{
    LastNodeStatus = nodeStatus;
    m_LastStatusReason = StatusReason;
    OnNodeStatusChanged(this);
}
```
## Credits
1. This project uses icons from https://game-icons.net/
