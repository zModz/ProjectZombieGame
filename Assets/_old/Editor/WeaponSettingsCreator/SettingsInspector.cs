using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line)
    {
        WeaponManager obj = EditorUtility.InstanceIDToObject(instanceId) as WeaponManager;
        if(obj != null)
        {
            WeaponSettingsMenu.Open(obj);
            return true;
        }
        return false;
    }
}



[CustomEditor(typeof(WeaponManager))]
public class SettingsInspector : Editor
{
    /*public override void OnInspectorGUI()
    {
        if (GUILayout.Button("OpenEditor"))
        {
            WeaponSettingsMenu.Open((WeaponManager)target);
        }
    }*/
}
