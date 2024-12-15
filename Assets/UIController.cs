using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void GameSceneJump()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void TitleSceneJump()
    {
        SceneManager.LoadScene("Title");
    }
}
