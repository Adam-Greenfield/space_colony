using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine; 

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    Editor shapeEditor;
    Editor colorEditor;
    Editor waterEditor;
    Planet planet;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
                planet.GeneratePlanet();
        }

        if (GUILayout.Button("Generate Planet"))
                planet.GeneratePlanet();


        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
        DrawSettingsEditor(planet.waterSettings, planet.OnWaterSettingsUpdated, ref planet.waterSettingsFoldout, ref waterEditor);
        
    }
         

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if(check.changed && onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
