using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    UpdateUI UIUpdate;
    public int AddScore;

    private void OnMouseDown()
    {
        Debug.Log("Destroyed");

        UIUpdate = GameObject.Find("Canvas").gameObject.transform.Find("Score").GetComponent<UpdateUI>();
        UIUpdate.Updatescore(AddScore);

        Destroy(gameObject);
    }
    private void Start()
    {
        AddScore = 1;
    }
}
