using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(FclFileImporter))]
public class FclFileImporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FclFileImporter fclFileImporter = (FclFileImporter)target;

        // Create a layout with two buttons side by side
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Open File", GUILayout.Height(50)))
        {
            fclFileImporter.OpenSingleFile();
        }

        if (GUILayout.Button("Open Folder", GUILayout.Height(50)))
        {
            fclFileImporter.OpenBulkFolder();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();
        // Add a description text
        EditorGUILayout.LabelField("Description:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Choose File if you want to convert single file");
        EditorGUILayout.LabelField("Choose Folder if you want to convert the entire folder");
        EditorGUILayout.LabelField("The first dialog box is to point where your .fcl files are");
        EditorGUILayout.LabelField("The second dialog box is to choose where to save the .anim files");

        EditorGUILayout.EndVertical();
    }
}
