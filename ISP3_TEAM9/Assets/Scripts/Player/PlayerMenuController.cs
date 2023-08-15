using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private Animator playerAnimController;
    [SerializeField] private new Light2D light;

    private Rigidbody2D rb;

    public float minIntensity = 0f;
    public float maxIntensity = 1f;

    [Range(1, 50)]
    public int smoothing = 5;

    Queue<float> smoothQueue;
    float lastSum = 0;

    private bool fadeOutCG = false;
    private float counter = 0;

    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start()
    {
        playerAnimController = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        smoothQueue = new Queue<float>(smoothing);
        
        if (light == null)
        {
            light = GetComponent<Light2D>();
        }
    }

    void Update()
    {
        if (light == null)
        {
            return; 
        }
        
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        light.intensity = lastSum / (float)smoothQueue.Count;

        if (fadeOutCG)
        {
            counter += Time.deltaTime;

            if (uiCanvasGroup.alpha > 0.0f)
            {
                uiCanvasGroup.alpha = Mathf.Lerp(1, 0, counter / 0.5f);
            }
            else
            {
                counter = 0;
                fadeOutCG = false;
            }
        }
    }

    public void StartButtonClicked()
    {
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        fadeOutCG = true;

        yield return new WaitForSeconds(0.5f);

        playerAnimController.SetBool("walkStart", true);

        light.transform.localPosition -= new Vector3(0.085f, 0, 0);

        
    }
}
