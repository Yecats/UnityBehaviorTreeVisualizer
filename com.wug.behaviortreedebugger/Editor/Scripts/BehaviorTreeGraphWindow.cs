using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WUG.BehaviorTreeDebugger
{
    public class BehaviorTreeGraphWindow : EditorWindow
    {

        public static BehaviorTreeGraphWindow Instance;

        public static readonly string c_RootPath = "Packages/com.wug.behaviortreedebugger/Editor";
        public static readonly string c_RootPathData = "Assets/Behavior Tree Debugger (Beta)";
        public static readonly string c_WindowPath = $"{c_RootPath}/Windows/";
        public static readonly string c_DataPath = $"{c_RootPathData}/Resources";
        public static readonly string c_StylePath = $"{c_RootPath}/Resources/Styles/BTGraphStyleSheet.uss";
        public static readonly string c_SpritePath = $"{c_RootPath}/Sprites/";

        public static MiniMap MiniMap;
        public List<Type> ScriptsWithBehaviorTrees = new List<Type>();

        public static BehaviorTreeGraphView GraphView;
        public static DataManager SettingsData;
        private ToolbarMenu m_ActiveTreesInScene;
        private List<UnityEngine.Object> m_SceneNodes = new List<UnityEngine.Object>();
        private SettingsWindow m_SettingsWindow;

        [MenuItem("What Up Games/Behavior Tree Debugger")]
        public static void Init()
        {
            SettingsData = new DataManager();
            Instance = GetWindow<BehaviorTreeGraphWindow>();
            Instance.titleContent = new GUIContent("Behavior Tree Debugger (Beta)");
            Instance.minSize = new Vector2(500, 500);
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar();
            GenerateMiniMap();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(GraphView);
        }

        /// <summary>
        /// Generates a mini map for navigation
        /// </summary>
        private async void GenerateMiniMap()
        {
            await Task.Delay(100);

            MiniMap = new MiniMap { anchored = false };
            MiniMap.SetPosition(new Rect(10, 30, 200, 140));
            GraphView.Add(MiniMap);

            ToggleMinimap(SettingsData.DataFile.EnableMiniMap);

        }

        public void ToggleMinimap(bool visible)
        {
            MiniMap.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Generates the graph view 
        /// </summary>
        private void ConstructGraphView()
        {
            GraphView = new BehaviorTreeGraphView()
            {
                name = "Behavior Graph"
            };

            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);

        }

        /// <summary>
        /// Generates the top toolbar on the graph window
        /// </summary>
        private void GenerateToolBar()
        {
            var toolbar = new Toolbar();

            m_ActiveTreesInScene = new ToolbarMenu();
            m_ActiveTreesInScene.text = "Select Behavior Tree";

            Button scanScene = new Button(() =>
            {
                if (!EditorApplication.isPlaying)
                {
                    "Scanning only works when your game is running.".BTDebugLog();
                    return;
                }



                for (int i = m_ActiveTreesInScene.menu.MenuItems().Count - 1; i == 0; i--)
                {
                    m_ActiveTreesInScene.menu.RemoveItemAt(i);
                }
                
                ScanProjectForTreeReferences();

                if (ScriptsWithBehaviorTrees == null || ScriptsWithBehaviorTrees.Count == 0)
                {
                     return;
                }

                foreach (Type script in ScriptsWithBehaviorTrees)
                {
                    UnityEngine.Object[] objectsWithScript = GameObject.FindObjectsOfType(script);
                    m_SceneNodes = new List<UnityEngine.Object>();

                    foreach (var item in objectsWithScript)
                    {
                        //Need to filter out objects that trigger this twice. 
                        //This means that it will not load multiple behavior trees, but only the first one it finds
                        //TODO: Adjust this in the future if support for multiple trees on a single object is needed
                        if (m_SceneNodes.FirstOrDefault(x => x == item) == null)
                        {
                            m_ActiveTreesInScene.menu.InsertAction(m_ActiveTreesInScene.menu.MenuItems().Count, item.name, (e) =>
                                {
                                    UnityEngine.Object runtimeData = e.userData as UnityEngine.Object;

                                    m_ActiveTreesInScene.text = runtimeData.name;
                                    GraphView.ClearTree();

                                    //reflection to get the node that needs to be run
                                    PropertyInfo nodeToRun = runtimeData.GetType().GetProperty("BehaviorTree");

                                    //Draw the tree
                                    GraphView.LoadBehaviorTree(nodeToRun.GetValue(runtimeData) as NodeBase);
                                },
                                (e) => 
                                { 
                                    if (m_ActiveTreesInScene.text.Equals(e.name))
                                    {
                                        return DropdownMenuAction.Status.Checked;
                                    }

                                    return DropdownMenuAction.Status.Normal;
                                }, item);
                            m_SceneNodes.Add(item);
                        }
                    }
                }
            });
            scanScene.text = "Scan Scene";
            scanScene.tooltip = "Scan the scene for behavior trees to load";

            Button settingsButton = new Button(() => 
            {
                if (m_SettingsWindow != null)
                {
                    m_SettingsWindow.Close();
                }

                m_SettingsWindow = GetWindow<SettingsWindow>();
                m_SettingsWindow.titleContent = new GUIContent("Settings");
                m_SettingsWindow.minSize = new Vector2(500, 500);
            });

            settingsButton.text = "Settings";

            toolbar.Add(new ToolbarSpacer());
            
            toolbar.Add(m_ActiveTreesInScene);
            toolbar.Add(scanScene);
            toolbar.Add(settingsButton);

            rootVisualElement.Add(toolbar);
        }

        private void ScanProjectForTreeReferences()
        {
            ScriptsWithBehaviorTrees.Clear();
            

            Assembly defaultAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");

            ScriptsWithBehaviorTrees = defaultAssembly?.GetTypes().Where( x => x.IsClass && x.GetInterfaces().Contains(typeof(IBehaviorTree)) && x.IsSubclassOf(typeof(UnityEngine.Object))).ToList();

            if (ScriptsWithBehaviorTrees == null || ScriptsWithBehaviorTrees.Count == 0)
            {
                "Did not find any scripts that reference <b><color=white>IBehaviorTree</color></b>. Are they in a different assembly than Assembly-CSharp?".BTDebugLog();
            }
        }


    }
}
