using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{

    Noise noise = new Noise();
    NoiseSettings settings;

    public NoiseFilter(NoiseSettings settings)
    {
       this.settings = settings; 
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = (noise.Evaluate(point * settings.roughness + settings.center) + 1) * .5f;
        return noiseValue * settings.strength;
    }

}
