using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePoints : MonoBehaviour
{
    UpdateUI UIUpdate;
    public int TimePoints;
    public float Timer; 
    public float TimeToAdd; //adjust time to gain points while pressing
    public float TimeToDestroy; //destroys gameobject at given time
    

    private void Start()
    {
        TimeToDestroy += Time.time;
        TimePoints = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Touch")
        {
            //Debug.Log("entered");
        }   
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("staying1");

        if (collision.gameObject.tag == "Touch")
        {
            if (Time.time > Timer)
            {
                Timer += TimeToAdd;
                //Debug.Log("STAYING");
                //UIUpdate = GameObject.Find("Canvas").gameObject.transform.Find("Score").GetComponent<UpdateUI>();
                //UIUpdate.Updatescore(TimePoints);
            }
        }
    }
      
    

    private void Update()
        {
       
            if (Time.time > TimeToDestroy)
            {
           
            Destroy(gameObject);
               

            }
          

    }
 }

