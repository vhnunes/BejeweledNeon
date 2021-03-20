using UnityEngine;
using UnityEditor;

namespace BJW.Editor
{
    public class BejeweledGameEditor : EditorWindow
    {
        [MenuItem("BJW/Game Editor")]
        static void Init()
        {
            BejeweledGameEditor window = (BejeweledGameEditor)EditorWindow.GetWindow(typeof(BejeweledGameEditor));
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Space(5);
            GUILayout.Label("Game Setup", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Here you can controll all the game related data and variables.", 
                MessageType.Info);
            GUILayout.Space(20);
            
            EditorGUILayout.HelpBox("Speeds", MessageType.None);
            EditorGUILayout.FloatField("Gem Movement Speed", new float());
            GUILayout.Space(20);
        }
    }
}