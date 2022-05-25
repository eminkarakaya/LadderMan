using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int asynSceneIndex = 0;
    public Canvas startCanvas;
    public Canvas finishCanvas;

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
