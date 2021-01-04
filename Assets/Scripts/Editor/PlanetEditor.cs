using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine; 

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    Editor animalEditor;
    Editor shapeEditor;
    Editor colorEditor;
    Editor orbitEditor;
    Editor waterEditor;
    Editor treeEditor;
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


        DrawSettingsEditor(planet.animalSettings, planet.OnAnimalSettingsUpdated, ref planet.animalSettingsFoldout, ref animalEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
        DrawSettingsEditor(planet.orbitSettings, planet.OnOrbitSettingsUpdated, ref planet.orbitSettingsFoldout, ref orbitEditor);
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.waterSettings, planet.OnWaterSettingsUpdated, ref planet.waterSettingsFoldout, ref waterEditor);
        DrawSettingsEditor(planet.treeSettings, planet.OnTreeSettingsUpdated, ref planet.treeSettingsFoldout, ref treeEditor);
        
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
