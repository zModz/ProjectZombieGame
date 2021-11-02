using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtentedEditorWindow : EditorWindow
{
    protected SerializedObject serialObj;
    protected SerializedProperty currentProp;

    private string selectPropPath;
    protected SerializedProperty selectProp;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach(SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }

    protected void DrawSideBar(SerializedProperty prop)
    {
        foreach (SerializedProperty p in prop)
        {
            if (GUILayout.Button(p.displayName))
            {
                selectPropPath = p.propertyPath;
            }
        }

        if (!string.IsNullOrEmpty(selectPropPath))
        {
            selectProp = serialObj.FindProperty(selectPropPath);
        }
    }

    protected void Apply()
    {
        serialObj.ApplyModifiedProperties();
    }
}
