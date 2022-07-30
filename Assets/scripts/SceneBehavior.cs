using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneBehavior : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public Slider depthSlider;
    public Toggle statsToggle;
    public TextMeshProUGUI depthSliderLabel;

    public void InitiateAIGame()
    {
        UpdatePrefs();
        SceneManager.LoadScene("main");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void ToggleOptionsMenu()
    {
        if(mainMenu.activeInHierarchy)
        {
            bool showStatistics = PlayerPrefs.showStats;
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
            depthSlider.value = PlayerPrefs.searchDepth;
            statsToggle.isOn = showStatistics;
        }
        else
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }
    public void UpdatePrefs()
    {
        PlayerPrefs.searchDepth = (int)depthSlider.value;
        depthSliderLabel.text = "AI Search Depth: " + (int)depthSlider.value;
        PlayerPrefs.showStats = statsToggle.isOn;
    }
}
