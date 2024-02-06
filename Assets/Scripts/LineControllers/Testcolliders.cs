using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testcolliders : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hitcollision");
    }
}
