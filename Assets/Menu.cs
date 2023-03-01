using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Options;
    public GameObject Exits;

    public void StartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PrevGame ()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ClearTab()
    {
        Options.SetActive(false);
        Exits.SetActive(false);
    }

    public void ShowOptions ()
    {
        ClearTab();
        Options.SetActive(true);
        //Options.transform.localPosition = new Vector3(0,0,0);
    }

    public void ShowExits ()
    {
        ClearTab();
        Exits.SetActive(true);
        //Exits.transform.localPosition = new Vector3(0,0,0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}