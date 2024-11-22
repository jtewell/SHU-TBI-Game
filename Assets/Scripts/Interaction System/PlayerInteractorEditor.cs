#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerInteractor))]
public class InteractionControllerEditor : Editor
{
    private SerializedProperty radius;
    private SerializedProperty debug;
    private SerializedProperty playerObj;
    private SerializedProperty interactControllerUI;
    private SerializedProperty interactTextDisplay;

    private bool showObjectAssignments = false;
    private bool showPrefabAssignments = false;
    private void OnEnable()
    {
        radius = serializedObject.FindProperty("radius");
        debug = serializedObject.FindProperty("DistanceDebug");
        playerObj = serializedObject.FindProperty("Player");
        interactControllerUI = serializedObject.FindProperty("OnInteract");
        interactTextDisplay = serializedObject.FindProperty("ActionText");

    }

    private void OnDisable()
    {
        
    }
    void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
        // Only assign the value back if it was actually changed by the user.
        // Otherwise a single value will be assigned to all objects when multi-object editing,
        // even when the user didn't touch the control.
        if (EditorGUI.EndChangeCheck())
        {
            property.floatValue = newValue;
        }
        EditorGUI.EndProperty();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Only use one of this script per scene.", MessageType.Warning);
        
        EditorGUILayout.HelpBox("This script controls and handles interactions between the player and Interactible objects in the scene.\n" +
                                "To make an object an interaction target, just add an Interactable script to them and a Collider.\n" +
                                "All Object settings are to be setup in the Interactable object.\n", MessageType.Info);
        
        
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Slider(radius, 0, 30, new GUIContent("Interaction Radius"));
        EditorGUILayout.PropertyField(debug, new GUIContent("Show Debug Gizmos"));
        EditorGUILayout.EndVertical();

        showObjectAssignments = EditorGUILayout.Foldout(showObjectAssignments, new GUIContent("Scene Object Assignments"), true);
        if (showObjectAssignments)
        {
            EditorGUILayout.BeginVertical("FrameBox");
            EditorGUILayout.PropertyField(playerObj, new GUIContent("Player Position Object"));
            
            EditorGUILayout.EndVertical();
        }
        showPrefabAssignments = EditorGUILayout.Foldout(showPrefabAssignments, new GUIContent("Prefab Links"), true);
        if (showPrefabAssignments)
        {
            EditorGUILayout.HelpBox("You will seldom need to edit this section. Talk to Kitsune before doing so.", MessageType.Error);
            EditorGUILayout.BeginVertical("FrameBox");
            EditorGUILayout.PropertyField(interactControllerUI, new GUIContent("Interaction Controller Toggled UI"));
            EditorGUILayout.PropertyField(interactTextDisplay, new GUIContent("UI Interaction Text"));
            EditorGUILayout.EndVertical();
        }


        //Apply Changes because unity is dumdum
        serializedObject.ApplyModifiedProperties();
    }
    
    
    
    
    
}

#endif