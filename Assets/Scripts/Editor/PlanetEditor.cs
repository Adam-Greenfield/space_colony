using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    Planet planet;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(planet.shapeSettings);
        DrawSettingsEditor(planet.colorSettings);
        
    }
         

    void DrawSettingsEditor(Object settings)
    {
        Editor editor = CreateEditor(settings);
        editor.OnInspectorGUI();
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
