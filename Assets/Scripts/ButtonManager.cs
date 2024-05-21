using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public SettingsData settings;
    // Start is called before the first frame update
    void Start()
    {
        settings.PreviousSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    public void BackToMenu()
    {               
        SceneManager.LoadScene("MainMenu");        
    }
    public void Respawn()
    {        
        SceneManager.LoadScene("L1");             
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
