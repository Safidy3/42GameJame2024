using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryScript : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            SceneManager.LoadSceneAsync("SampleScene");
    }
}
