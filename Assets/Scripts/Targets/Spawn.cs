using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour

    
{
    public float Timer;
    public float TimeToSpawn;
    public float TomatoSpawn;
    public float TomatoTimer;
    //Dragline spawn points
    [SerializeField] float MinX = -5;
    [SerializeField] float MaxX = 5;
    [SerializeField] float MinY = -5;
    [SerializeField] float MaxY = 5;

    //tomato spawn points
    [SerializeField] float TomatoMinX = -5;
    [SerializeField] float TomatoMaxX = 5;
    [SerializeField] float TomatoMinY = -5;
    [SerializeField] float TomatoMaxY = 5;

    

    [SerializeField] GameObject[] SlideObject;
    [SerializeField] GameObject Tomato;



    private void Update()
    {
        if (Time.time > Timer)
        {
            int randEnemy = Random.Range(0, SlideObject.Length);
            float x = Random.Range(MinX, MaxX);
            float y = Random.Range(MinY, MaxY);
            Timer += TimeToSpawn;
            
          Instantiate(SlideObject[randEnemy], new Vector3(x, y, 0), Quaternion.identity);
         
        }
        if (Time.time > TomatoTimer)
        {
            float x = Random.Range(TomatoMinX, TomatoMaxX);
            float y = Random.Range(TomatoMinY, TomatoMaxY);
            TomatoTimer += TomatoSpawn;
            Instantiate(Tomato, new Vector3(x, y, 0), Quaternion.identity);
        }

    }

}


