using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehavior : MonoBehaviour
{
    public void InitiateAIGame()
    {
        SceneManager.LoadScene("main");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
