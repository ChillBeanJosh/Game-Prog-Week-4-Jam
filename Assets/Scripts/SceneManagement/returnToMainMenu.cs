using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class returnToMainMenu : MonoBehaviour
{

    public KeyCode mainMenuKey;


    private void Update()
    {
        if (Input.GetKeyDown(mainMenuKey))
        {
            sceneManager.Instance.LoadMainMenu();
        }
    }
}
