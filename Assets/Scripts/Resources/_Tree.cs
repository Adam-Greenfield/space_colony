using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Tree : MonoBehaviour
{
    [SerializeField]
    private int age;
    private bool runtime = false;
    float scaleDuration = 1.0f;
    public Vector3 defaultScale;
    [SerializeField]
    public int Age
    {
        get
        {
            return age;
        }
        set
        {
            if (value <= 4)
            {
                age = value;
                if(runtime)
                    StartCoroutine(SmoothScaleTree());
            }
        }
    }

    void Start()
    {
        runtime = true;
        InvokeRepeating("IncrementAge", 2.0f, 4.0f);
    }

    public void SetStartingScale()
    {
        transform.localScale = defaultScale / AgeScaleFraction();
    }

    IEnumerator SmoothScaleTree()
    {
        Vector3 actualScale = transform.localScale;
        Vector3 targetScale = defaultScale / AgeScaleFraction();

        //wait here to space ot growth
        yield return new WaitForSeconds(Random.Range(0.1f, 4.0f));

        for(float t = 0; t < 1; t += Time.deltaTime / scaleDuration )
        {
            transform.localScale = Vector3.Lerp(actualScale ,targetScale ,t);
            yield return null;
        }
    }

    float AgeScaleFraction()
    {
        //takes age as a number between 1 and 100, then transforms to new number in range of 10 to 35, 
        //which is the scale we want for our planet
        //the 60 - is so we get the inverse
        //using 20 and 40 as scale
        return 70 - ((((Age - 1) * (30)) / (3)) + 20);
    }

    void IncrementAge()
    {
        if(Random.Range(0,4) <= 3)
            Age ++;
    }
}
