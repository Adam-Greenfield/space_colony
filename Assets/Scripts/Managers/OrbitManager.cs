using UnityEngine;
using System.Linq;

public class OrbitManager : MonoBehaviour
{
    OrbitSettings settings;

    GameObject primary;

    public void UpdateSettings(OrbitSettings settings, GameObject primary)
    {
        this.settings = settings;
        this.primary = primary;
    }

    public void SetRange()
    {
        float primarySize = primary.transform.localScale.x;
        int distance = (int)settings.range;

        int fraction = Random.Range(1,10);

        Vector3 distanceVector = new Vector3(
            distance * fraction / 10 * (Random.Range(0,2)*2-1),
            Random.Range(-5, 5),
            distance * (10 - fraction) / 10 * (Random.Range(0,2)*2-1));

        transform.position = primary.transform.position + distanceVector;
        //set rotation
        transform.Rotate(new Vector3(Random.Range(-5,5), 0, Random.Range(-5,5)), Space.Self );
    }

    void Update()
    {
        float primarySize = primary.transform.localScale.x;
        int distance = (int)settings.range;

        int largestRange = (int)System.Enum.GetValues(typeof(Range)).Cast<Range>().Max();
        //rotate around primary
        transform.RotateAround(primary.transform.position, primary.transform.up, (largestRange - distance) / 300 * Time.deltaTime);

        //rotate around axis
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * distance / 100);
    }

}