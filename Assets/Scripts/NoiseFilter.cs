using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    Noise noise = new Noise();

    public float Evaluate(Vector3 point)
    {
        return (noise.Evaluate(point) + 1) * .5f;
    }

}
