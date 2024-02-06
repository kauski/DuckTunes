using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathColliders : MonoBehaviour
{
    
    public bool FirstPoint;
    public bool SecondPoint;
    UpdateUI UIUpdate;
    [SerializeField] int GainFirstTomato;
    [SerializeField] int GainMidTomato;
    [SerializeField] int GainLastTomato;

    GameObject[] Midpoints;
    public int MidDone;

    
    private void Start()
    {
        FirstPoint = false;
        SecondPoint = false;
        MidDone = 0;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "FirstPoint")
        {

            Midpoints = GameObject.FindGameObjectsWithTag("MidPoint");

                FirstPoint = true;
                Debug.Log("Activated");
                UIUpdate = GameObject.Find("Canvas").gameObject.transform.Find("Score").GetComponent<UpdateUI>();
                UIUpdate.Updatescore(GainFirstTomato);
            
        }
      
        if (collision.gameObject.tag == "MidPoint" && FirstPoint == true)

         {
            MidDone += 1;
            UIUpdate = GameObject.Find("Canvas").gameObject.transform.Find("Score").GetComponent<UpdateUI>();
            UIUpdate.Updatescore(GainMidTomato);
            
                if (MidDone == Midpoints.Length)
                {
                    SecondPoint = true;
                    Debug.Log("LastPointActivated");
                }
        }
        if (collision.gameObject.tag == "LastPoint" && SecondPoint == true )
        {
            Debug.Log("Touched last point");
            UIUpdate = GameObject.Find("Canvas").gameObject.transform.Find("Score").GetComponent<UpdateUI>();
            UIUpdate.Updatescore(GainLastTomato);
            FirstPoint = false;
            SecondPoint = false;
            MidDone = 0;
            Destroy(GameObject.FindWithTag("Path"));
        }
        }
    private void OnTriggerExit2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Path")
        {
            Destroy(GameObject.FindWithTag("Path"));
            MidDone = 0;
        }
    }


}
    



