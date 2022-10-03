using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Gameover : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene("Boxing");
    }
    public void LoadMenu(){
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame(){
        Debug.Log("Quit game");
        Application.Quit();
    }
}
