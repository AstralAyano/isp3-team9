using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadMenuToLobby : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;

    private float onePercent = 0;

    void Awake()
    {
        onePercent = (endPos.y - startPos.y) / 100;
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        //RandomHintText();

        await Task.Delay(100);
        StartCoroutine("LoadingCanvas", scene);
    }

    IEnumerator LoadingCanvas(AsyncOperation scene)
    {
        do
        {
            yield return new WaitForSeconds(0.1f);
            
            float distFromStart = onePercent * scene.progress;

            Vector2 destination = startPos + new Vector2(0, distFromStart);

            if (transform.position.y < destination.y)
            {
                gameObject.GetComponent<PlayerMenuController>().playerAnimController.SetBool("walkStart", true);
                gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, (destination.y - transform.position.y) * 2);
            }
        }
        while (scene.progress < 0.9f);

        yield return new WaitForSeconds(1);

        scene.allowSceneActivation = true;

        yield return new WaitForSeconds(0.2f);
    }
}
