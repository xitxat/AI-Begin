﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// MUST be in Folder: "Editor"
//  RENAMES: Dic


[CustomEditor(typeof(GAgentVisual))]
[CanEditMultipleObjects]
public class GAgentVisualEditor : Editor 
{


    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        GAgentVisual agent = (GAgentVisual) target;
        GUILayout.Label("Name: " + agent.name);
        GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().currentAction);
        GUILayout.Label("Actions: ");
        foreach (GAction a in agent.gameObject.GetComponent<GAgent>().actions)
        {
            string pre = "";
            string eff = "";

            foreach (KeyValuePair<string, int> p in a.preconditionsDic)
                pre += p.Key + ", ";
            foreach (KeyValuePair<string, int> e in a.postconditionsDic)
                eff += e.Key + ", ";

            GUILayout.Label("====  " + a.actionName + "(" + pre + ")(" + eff + ")");
        }
        GUILayout.Label("Goals: ");
        foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GAgent>().goals)
        {
            GUILayout.Label("---: ");
            foreach (KeyValuePair<string, int> sg in g.Key.sgoalsDic) // rename sGoals
                GUILayout.Label("=====  " + sg.Key);
        }
        serializedObject.ApplyModifiedProperties();
    }
}