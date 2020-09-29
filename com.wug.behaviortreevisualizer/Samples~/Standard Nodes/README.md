
# Standard Behavior Tree Nodes
This sample project contains a set of standard behavior tree nodes that can be used for your game. Here's what you need to know:

1. There are 11 specialized composite and decorator nodes 
2. When creating new nodes, make sure to inherit from `Composite`, `Condition` and `Decorator` when creating new nodes of those types. 
3. Action/Leaf nodes can be inherited from `Node` directly.
4. A settings file has been included but needs to be moved to **Behavior Tree Visuallizer (Beta)\Resources** for the tool to pick it up.

## General
There are two general node types:

| Name      	| Description                                                                                                                                       	|
|-----------	|---------------------------------------------------------------------------------------------------------------------------------------------------	|
| Node      	| Base class for all node types - Everything should ultimately derive from this.                                                                    	|
| Condition 	| Base class for all condition node types. Does not do anything special. Exists so that you can stylize all conditions in the tool easily. 	|

## Composites
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

## Decorators
Decorators are a way to alter how an individual node is run. These can range from changing the return status code (`Inverter`) to defining how many times to excute (`Repeater`).

| Name      	| Description                                                                                                 	|
|-----------	|-------------------------------------------------------------------------------------------------------------	|
| Decorator 	| Base class for all decorator node types. Supports a single child.                                           	|
| Inverter  	| Inverts the result of the node - `Success` will return `Failure` and `Failure` will return `Success`. 	|
| Repeater  	| Run the child node a specified amount of times before exiting.                                              	|
| Succeeder 	| Returns `Running` status if running otherwise returns the `Success` status. Never the `failure` status.     	|
| Timer     	| Run the child node a specified amount of time in seconds.                                                   	|
| UntilFail 	| Continues to run the child node until the child returns a status of `Failure`.                              	|