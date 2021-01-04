using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;


[CreateAssetMenu()]
public class AnimalSettings : ScriptableObject
{
    public Species[] species;

    [System.Serializable]
    public class Species
    {
        [ClassExtends(typeof(Animal))]
        public ClassTypeReference animalType;
        public string speciesName;
        public int population;
        public GameObject model;

    }
}


