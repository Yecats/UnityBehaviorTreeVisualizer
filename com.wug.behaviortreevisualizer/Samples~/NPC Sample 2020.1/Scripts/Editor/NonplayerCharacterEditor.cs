using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;

namespace WUG.BehaviorTreeVisualizer
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
