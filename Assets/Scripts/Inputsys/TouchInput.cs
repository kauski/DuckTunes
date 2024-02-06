using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    UpdateUI UIUpdate;
   public int TimePoints;
   

    private void Start()
    {
        TimePoints = 1;
    }
  
    void FixedUpdate()
    { 
    
        for (int i = 0; i < Input.touchCount; i++)
        {
            
           
            RaycastHit hitinfo = new RaycastHit();
           
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hitinfo)) 
            {

                Debug.Log($"{hitinfo.collider.name}");
                if ((hitinfo.collider.tag == "Tomato"))
                {
                    Debug.Log("hit tomato");
                }
            }
        }

    }

    public void Hitto()
    {
        
    }
  
}
