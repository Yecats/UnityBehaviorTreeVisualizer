# Changelog

All notable changes to this package will be documented in this file.

## [0.1.4] - 10-16-2020

* [#24](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/24) Added a new Decorator Node called "Delay". It will run a timer before running the child node once.
* [#25](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/25) Added a new setting option to display the last evaluated timestamp on the Node. This is on by default.
* Removed the irrelevant default options on the context menu of the graph.

## [0.1.3] - 10-04-2020

* [#21](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/21) You can now right click on a graph node to launch any of the scripts it represents.
* [#18](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/18) & [#15](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/15) Added a hook so that you can request a redraw of the graph. Useful for dynamic trees or to add an inspector button (now included in the samples).
* [#20](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/20) Added a **Community Nodes** sample folder so that anyone can contribute behavior tree nodes! Also added a node - NavigateToDestination, which will take a GameObject or NavMeshAgent and position reference and handle movement.

## [0.1.2] - 9-29-2020

* Fixed a bug that was causing the layout to be drawn incorrectly (no spaces between sections).
* Refactored to support new name.
* Addressed minor warnings in the style sheet.
* Removed `m_LastNodeStatus` from `NodeBase` as it is not required in all scenarios.

## [0.1.1] - 9-23-2020

* Added support for 2019.4 LTS for the sample projects
* [#9](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/9) Settings file no longer loses data on editor restart.
* [#6](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/6) Settings Window - Checking the decorator checkbox now disables the color field.
* [#4](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/4) Behavior Tree now works when scene stops/starts.
* [#1](https://github.com/Yecats/UnityBehaviorTreeVisualizer/issues/1) Action nodes that have decorators now properly highlight.
* Decorator nodes with `StatusReason` messages now display their messages.
* Nodes now show their last drawn state when first launching the Behavior Tree Window.
* Fix null ref issue when icons are missing.
* Settings file now contains default values for highlight, dim, and color fields.

## [0.1.0] - 09-15-2020

### This is the first release of _Behavior Tree Visualizer.

* Draws a graph representation of your behavior tree implementation
* Highlights nodes as they are running and surfaces StatusReason message
* Several style customization settings
* Standard behavior tree nodes for new implementation
* Sample project demonstrating behavior tree usage
