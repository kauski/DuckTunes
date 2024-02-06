using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
         //   gameObject.GetComponent<TrailRenderer>().enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
           // gameObject.GetComponent<TrailRenderer>().enabled = false;
        }
    }
}
