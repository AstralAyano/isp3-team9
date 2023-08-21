using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Vector2 startPos = new Vector2(-4, 0);
    private Vector2 endPos = new Vector2(4.5f, 0);

    private float onePercent;

    private static Action onLoaderCallback;

    private void Awake()
    {
        onePercent = (endPos.y - startPos.y) / 100;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        onLoaderCallback = () => {
            Loading(sceneName);
        };
        
        SceneManager.LoadScene("SceneLoading");
        Debug.Log("Loading screen");
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    public async void Loading(string sceneName)
    {
        AsyncOperation targetScene = SceneManager.LoadSceneAsync(sceneName);
        targetScene.allowSceneActivation = false;

        //RandomHintText();

        await Task.Delay(100);
        Debug.Log("Start coroutine");
        StartCoroutine(LoadAsync(targetScene));
    }

    private IEnumerator LoadAsync(AsyncOperation targetScene)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        do
        {
            yield return null;

            Debug.Log(player.transform.position);

            float distFromStart = onePercent * targetScene.progress;

            Vector2 destination = startPos + new Vector2(0, distFromStart);

            if (player.transform.position.y < destination.y)
            {
                player.GetComponent<Rigidbody2D>().velocity += new Vector2(0, (destination.y - player.transform.position.y) * 2);
            }
        }
        while (targetScene.progress < 0.9f);

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        targetScene.allowSceneActivation = true;

        yield return new WaitForSeconds(0.2f);
    }
}