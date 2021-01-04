using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sword))]
public class SwordEditor : Editor
{
    SerializedProperty jabAnimation;
    SerializedProperty swingAnimation;    
    private void OnEnable() {
        EditorUtility.SetDirty(serializedObject.targetObject);
        jabAnimation = serializedObject.FindProperty("JabAnimation");
        swingAnimation = serializedObject.FindProperty("SwingAnimation");
    }
    override public void OnInspectorGUI(){
       // EditorUtility.SetDirty(target);
        var myScript = target as Sword;
        DrawDefaultInspector();
        serializedObject.Update();

        if(myScript.actions.isEquipable){
            myScript.isJabWeapon = EditorGUILayout.Toggle("IsJabWeapon", myScript.isJabWeapon);
            if(myScript.isJabWeapon){
                EditorGUILayout.ObjectField(jabAnimation);
            }
            myScript.isSwingWeapon = EditorGUILayout.Toggle("IsSwingWeapon", myScript.isSwingWeapon);
            if(myScript.isSwingWeapon){
                EditorGUILayout.ObjectField(swingAnimation);
            }
        }
        else{
            myScript.isJabWeapon = false;
            myScript.isSwingWeapon = false;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
