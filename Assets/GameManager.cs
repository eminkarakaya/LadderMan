using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{   
    public AudioClip winSound;
    float finishTime = 1.5f;
    [SerializeField] Text winText;
    int scoreEarned;
    public bool isGameOver;
    [SerializeField] _SceneManager _sceneManager;
    StickMan stickMan;
    public Text scoreText;
    public Text ladderText;
    public GameObject startCanvas;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    int currScore;

    void OnEnable()
    {
        _sceneManager = GameObject.FindObjectOfType<_SceneManager>();
        StickMan.GameOverEvent += GameOver;
        StickMan.WinEvent += SetScore;
        StickMan.WinEvent += Win;
        StickMan.CollectLadder += SetLadderText;
        StickMan.UseLadder += SetLadderText;
    }
    void OnDisable()
    {
        StickMan.GameOverEvent -= GameOver;
        StickMan.WinEvent -= Win;
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
        _sceneManager = GameObject.FindObjectOfType<_SceneManager>();
        Debug.Log(_sceneManager.asynSceneIndex);
        if(SceneManager.sceneCountInBuildSettings == _sceneManager.asynSceneIndex+1)
        {
            SceneManager.UnloadSceneAsync(_sceneManager.asynSceneIndex);
            _sceneManager.asynSceneIndex = 0;
            SceneManager.LoadSceneAsync(_sceneManager.asynSceneIndex, LoadSceneMode.Single);
            return;
        }
        _sceneManager.asynSceneIndex++;
        SceneManager.LoadSceneAsync(_sceneManager.asynSceneIndex,LoadSceneMode.Single);
    }
    public void RetryBtn()
    {
        SceneManager.LoadSceneAsync(_sceneManager.asynSceneIndex,LoadSceneMode.Single);
    }
    public IEnumerator WinGame()
    {
        isGameOver = true;
        winCanvas.SetActive(true);
        yield return new WaitForSeconds(finishTime);
        winCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }
    public IEnumerator LoseGame()
    {
        isGameOver = true;
        loseCanvas.SetActive(true);
        yield return new WaitForSeconds(1);
        loseCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void GameOver()
    {
        StartCoroutine(LoseGame());
    }
    public void Score()
    {
        DOTween.To(GetScore,SetScore,scoreEarned,finishTime).SetEase(Ease.Unset);
    }
    int GetScore()
    {
        return currScore;
    }
    void SetScore(int value)
    {
        currScore += value;
        winText.text = "Kazanılan Puan : " + value;
    }
    public void Win(GameObject other)
    {
        scoreEarned = other.GetComponent<Puan>().puan;
        Score();
        AudioSource.PlayClipAtPoint(winSound,Camera.main.transform.position);
        Debug.Log(other.gameObject.name);
        //winText.text = "Kazanılan Puan : " + other.GetComponent<Puan>().puan.ToString();
        StartCoroutine(WinGame());
    }

}
   
