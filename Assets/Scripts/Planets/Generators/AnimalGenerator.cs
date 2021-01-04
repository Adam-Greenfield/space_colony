using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeReferences;
public class AnimalGenerator
{
    AnimalSettings settings;

    public void UpdateSettings(AnimalSettings settings)
    {
        this.settings = settings;
    }

    public void InstantiateAnimal(Vector3 vertex, float elevation, IPrimary primary, float waterElevation)
    {
        AnimalSettings.Species species = settings.species[UnityEngine.Random.Range(0, settings.species.Length - 1)];

        var type = Activator.CreateInstance(species.animalType);

        if(type is Fish fishType)
        {
            if (elevation < waterElevation)
            {
                GameObject animalGO = GameObject.Instantiate(species.model, vertex, Quaternion.identity) as GameObject;
                animalGO.transform.parent = primary.transform;

                Fish fish = animalGO.GetComponent<Fish>();
                fish.primary = primary;
            }
        }

        if(type is Mamal mamalType)
        {
            if (elevation > waterElevation)
            {
                GameObject animalGO = GameObject.Instantiate(species.model, vertex, Quaternion.identity) as GameObject;
                animalGO.transform.parent = primary.transform;

                Mamal mamal = animalGO.GetComponent<Mamal>();
                mamal.primary = primary;
            }
        }
    }
}
