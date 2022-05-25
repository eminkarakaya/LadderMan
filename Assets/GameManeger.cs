using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    private int asynSceneIndex = 1;
    public Canvas startCanvas;
    public Canvas finishCanvas;

    public void NextLevelButton()
    {
        if(SceneManager.sceneCountInBuildSettings == asynSceneIndex+1)
        {
            SceneManager.UnloadSceneAsync(asynSceneIndex);
            asynSceneIndex++;
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Single);
        }
        else
        {
            if(SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(asynSceneIndex);
                asynSceneIndex++;
            }
        }
        SceneManager.LoadSceneAsync(asynSceneIndex,LoadSceneMode.Single);
    }
}
