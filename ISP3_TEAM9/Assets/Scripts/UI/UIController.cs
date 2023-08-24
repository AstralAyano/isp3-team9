using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public static int floorNum = 1;

    [SerializeField] private List<GameObject> uiList = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public void CheckNextScene(Scene nextScene)
    {
        Debug.Log(nextScene.name);

        if (nextScene == SceneManager.GetSceneByName("SceneMenu") ||
            nextScene == SceneManager.GetSceneByName("SceneLoading") ||
            nextScene == SceneManager.GetSceneByName("SceneEnd") ||
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SceneEnd"))
        {
            uiList[0].SetActive(false);
            uiList[1].SetActive(false);
            uiList[2].SetActive(false);
        }
        else if (nextScene == SceneManager.GetSceneByName("SceneLobby") ||
                nextScene == SceneManager.GetSceneByName("SceneLevel") ||
                nextScene == SceneManager.GetSceneByName("SceneLevelBoss"))
        {
            uiList[0].SetActive(true);
            uiList[1].SetActive(false);
            uiList[2].SetActive(true);
        }
    }
}
