using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    StickMan stickMan;
    public Text scoreText;
    public Text ladderText;
    private int asynSceneIndex = 0;
    public Canvas startCanvas;
    public Canvas finishCanvas;

    void OnEnable()
    {
        StickMan.WinEvent += SetScore;
        StickMan.CollectLadder += SetLadderText;
        StickMan.UseLadder += SetLadderText;
    }
    void OnDisable()
    {
        StickMan.WinEvent -= SetScore;
        StickMan.CollectLadder -= SetLadderText;
        StickMan.UseLadder -= SetLadderText;
    }
    void Start()
    {
        stickMan = GameObject.FindObjectOfType<StickMan>();
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score"));
        
    }
    public void SetScore(GameObject other)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score")+ other.gameObject.GetComponent<Puan>().puan);
    }
    public void SetLadderText()
    {
        ladderText.text = stickMan.laddersOnTheBackCount.ToString();
    }
    public void NextLevelButton()
    {
        if(SceneManager.sceneCountInBuildSettings == asynSceneIndex+1)
        {
            SceneManager.UnloadSceneAsync(asynSceneIndex);
            asynSceneIndex = 0;
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Single);
        }
        // else
        // {
        //     if(SceneManager.sceneCount > 1)
        //     {
        //         SceneManager.UnloadSceneAsync(asynSceneIndex);
        //         
        //     }
        // }
        asynSceneIndex++;
        SceneManager.LoadSceneAsync(asynSceneIndex,LoadSceneMode.Single);
    }
}
