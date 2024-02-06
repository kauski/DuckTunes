using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Options : MonoBehaviour
{
    public GameObject[] Menus;


    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button Return = root.Q<Button>("Return");

        Return.clicked += this.ReturnButtonOnClicked;
    }

    private void ReturnButtonOnClicked()
    {

        Menus[0].SetActive(true);
        this.gameObject.SetActive(false);
    }


}
