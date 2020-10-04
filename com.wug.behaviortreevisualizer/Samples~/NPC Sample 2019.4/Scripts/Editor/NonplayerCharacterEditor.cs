using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;

namespace Assets.Samples.Behavior_Tree_Visualizer__Beta_._0._1._2.NPC_Behavior_Tree___2019._4.Scripts.Editor
{
    [CustomEditor(typeof(NonPlayerCharacter))]
    public class NonplayerCharacterEditor : UnityEditor.Editor
    {
        private NonPlayerCharacter m_NPC;

        private void OnEnable()
        {
            m_NPC = target as NonPlayerCharacter;

            EditorApplication.update += RedrawView;
        }

        void RedrawView()
        {
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Draw Behavior Tree"))
            {
                m_NPC.ForceDrawingOfTree();
            }
        }
    }
}
