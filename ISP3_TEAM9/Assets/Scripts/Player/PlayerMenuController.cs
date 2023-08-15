using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
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

    private bool startWalking = false;

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
    }

    void LateUpdate()
    { 
        if (startWalking && transform.position.y < 4.35f)
        {
            float amt = 2.0f * Time.deltaTime;
            transform.position += new Vector3(0, amt, 0);
        }
    }

    public void StartButtonClicked()
    {
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        GameObject.Find("UIMenu").GetComponent<UIMenuController>().fadeOutCanvasGroup = true;

        yield return new WaitForSeconds(2.0f);

        playerAnimController.SetBool("walkStart", true);

        startWalking = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Door"))
        {
            startWalking = false;

            UIMenuController uiMenu = GameObject.Find("UIMenu").GetComponent<UIMenuController>();

            uiMenu.StartCoroutine(uiMenu.DoorTouched());
        }
    }
}
