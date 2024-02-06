using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);
            Instance = this;
        
        Load();
        
    }
    public void Save()
    {

        PlayerPrefs.SetString("Save",Helper.Serialize<SaveState>(state));
        
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("Save"))
        {
            Debug.Log("Savefile not created");
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            Debug.Log("Savefile created");
            state = new SaveState();
            Save();
            Debug.Log("Didnt find save file");
        }
    }
    
}
