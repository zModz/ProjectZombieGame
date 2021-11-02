using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class WeaponSettingsMenu : ExtentedEditorWindow
{
    public static void Open(WeaponManager manager)
    {
        WeaponSettingsMenu window = GetWindow<WeaponSettingsMenu>("Weapon Setings");
        window.serialObj = new SerializedObject(manager);
    }

    private void OnGUI()
    {
        currentProp = serialObj.FindProperty("weaponList");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSideBar(currentProp);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if(selectProp != null)
        {
            DrawProperties(selectProp, true);
        } else { EditorGUILayout.LabelField("Select an item from the list"); }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }
}
