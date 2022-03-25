using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    public Canvas startCanvas;
    public Canvas finishCanvas;

    public void NextLevelButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
