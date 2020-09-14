![beta](https://img.shields.io/badge/release-beta-orange) ![GitHub issues](https://img.shields.io/github/issues/Yecats/UnityBehaviorTreeDebugger) ![version](https://img.shields.io/badge/unity%20version-2020.1.0f1%2B-blue) 

![twitter](https://img.shields.io/twitter/follow/yecats131?style=social) ![Twitter](https://img.shields.io/twitter/follow/whatupgames?style=social)

# Unity Behavior Tree Debugger (Beta)
Behavior Trees are a fantastic way to write modular AI that can scale in complexity. Unfortunately, it can be quite hard to visualize how your tree is being executed which makes it difficult to debug potential failure points. The Behavior Tree Debugger tool was created to solve these problems! The tool will scan for active behavior trees in your scene and group them in a drop down for easy toggle. A graph will be drawn, and nodes will light up, showing you which part of the tree is currently running. 

## Features
1. **Customize the graph** by choosing the title bar color, the icon, amount to dim inactive nodes and more.
2. **Robust debug messages** can be viewed directly on the graph. Surface anything you want to see.
3. **Includes basic node types** to help you get up and running quickly. No need to write a sequencer, selector, inverter, or more!

## What's Included?
This package comes with:

1. Behavior Tree Debugger tool built with Unity Toolkit (formerly UI Elements)
2. Standard Behavior Tree nodes to get you up and running quickly
3. Sample project to demonstrate the implementation

### Standard Behavior Tree Nodes
There are 11 composite and decorator nodes that can be used in your game. Inherit from `Composite`, `Condition` and `Decorator` when creating new nodes of those types. Action/Leaf nodes can be inherited from `Node` directly.

#### General
There are two general node types:

| Name      	| Description                                                                                                                                       	|
|-----------	|---------------------------------------------------------------------------------------------------------------------------------------------------	|
| Node      	| Base class for all node types - Everything should ultimately derive from this.                                                                    	|
| Condition 	| Base class for all condition node types. Does not do anything special. Exists so that you can stylize all conditions in the debugger tool easily. 	|

#### Composites
Composites are essentially instructions for your behavior tree on how it should run a group of child nodes. 

| Name                 	| Description                                                                                                                                                                                                         	|
|----------------------	|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| Composite            	| Base class for all composite node types.                                                                                                                                                                            	|
| Selector             	| Runs each child until one of them returns status Success. Will return failure if no child runs successfully.                                                                                                        	|
| RandomSelector       	| Shuffles the child nodes the first time the behavior tree is constructed (only once). Executes the same as `Selector`                                                                                               	|
| AlwaysRandomSelector 	| Shuffles the child nodes every time it runs for the first time. Executes the same as `Selector`.                                                                                                                    	|
| Sequence             	| Will continue to run if all children return the status of Running or Success. Return a status code of Failure if any children return status Failure. Returns status code of Success if all children return Success. 	|
| RandomSequence       	| Shuffles the child nodes the first time the behavior tree is constructed (only once). Executes the same as `Sequence`.                                                                                              	|
| AlwaysRandomSequence 	| Shuffles the child nodes every time it runs for the first time. Executes the same as `Sequence`.                                                                                                                    	|
#### Decorators
Decorators are a way to alter how an individual node is run. These can range from changing the return status code (`Inverter`) to defining how many times to excute (`Repeater`).

| Name      	| Description                                                                                                 	|
|-----------	|-------------------------------------------------------------------------------------------------------------	|
| Decorator 	| Base class for all decorator node types. Supports a single child.                                           	|
| Inverter  	| Inverts the final result of the node - `Success` will return `Failure` and `Failure` will return `Success`. 	|
| Repeater  	| Run the child node a specified amount of times before exiting.                                              	|
| Succeeder 	| Returns `Running` status if running otherwise returns the `Success` status. Never the `failure` status.     	|
| Timer     	| Run the child node a specified amount of time in seconds.                                                   	|
| UntilFail 	| Continues to run the child node until the child returns a status of `Failure`.                              	|

## Setup
Here are the most important things to know:

1. Your base Node must inherit from `NodeBase` for the tool to pick up any running trees in your scene.
2. Your node should call the `OnNodeStatusChanged` method when the node's status has been changed. This is what the tool listens to, to know which nodes should be highlighted, what their status is, and whether to draw any debug messages.
3. By inheriting `NodeBase` all nodes will have the notion of child node(s). Do not add anything to the list, and it'll be ignored (and thus, treated as a leaf node). Decorators (inverter, untilfail, etc.) should only ever have one child and composites (seqencer, selector, etc.) can have as many as they need.

### New Behavior Tree


### Example - Call OnNodeStatusChanged only when a new status or message occurs
1. Your base class must inherit from the `NodeBase` class and implement `IBehaviorTree`.

```csharp

```

2. Call `OnNodeStatusChanged` event when the node's status has been changed. In my example, I call it only if the all up status has changed (i.e. `Running` to `Success`) or if the `StatusReason` has changed. Here's an example:

``` csharp
if (LastNodeStatus != nodeStatus || !m_LastStatusReason.Equals(StatusReason))
{
    OnNodeStatusChanged(nodeStatus, StatusReason);
    LastNodeStatus = nodeStatus;
    m_LastStatusReason = StatusReason;
}
```

## Error Messages

#### Did not find any scripts that reference IBehaviorTree. Are they in a different assembly than Assembly-CSharp?
There are two reasons that you may encounter this error. The first is that your base node does not implement `IBehaviorTree`, and thus no references can be detected. Make sure you implmenet the interface.

The second reason is that your scripts are not in the default `Assembly-CSharp` assembly. For performance reasons, the Behavior Tree Debugger only scans the default assembly for references to `IBehaviorTree`. It is possible to seperate your game into multiple assemblies and if you have done this, you will need to update the tool to include a reference to the assembly(s). This can be done in `BehaviorTreeGraphWindow.ScanProjectForTreeReferences()`. 

#### Scanning only works when your game is running
The **Scan Scene** button on the toolbar is used to scan for behavior trees actively running in your scene. You'll see this error message if you click on the button when not in play mode. 

## Credits
1. This project uses icons from https://game-icons.net/.
