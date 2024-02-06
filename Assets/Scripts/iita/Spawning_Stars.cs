using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning_Stars : MonoBehaviour
{

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject echo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwSpawns <= 0)
        {
            Instantiate(echo, transform.position, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
        } else
        {
            timeBtwSpawns -= Time.deltaTime;

        }
    }
}
