using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public static MessageController instance;

    public TextMeshProUGUI messageText;
    public float fadeDelay;
    public float fadeSpeed;

    private Color initialColour;

    public string currentMessage = null;

    public Queue<string> messageQueue = new Queue<string>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        currentMessage = null;
        initialColour = messageText.color;
    }

    public void EnqueueMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

    private void Update()
    {
        if (currentMessage == null && messageQueue.Count > 0)
        {
            currentMessage = messageQueue.Dequeue();
            DisplayMessage(currentMessage);
        }
    }

    public void DisplayMessage(string message)
    {
        StopAllCoroutines();
        messageText.text = message;
        messageText.color = initialColour;
        StartCoroutine(DisplayMessageCoroutine());
    }

    IEnumerator DisplayMessageCoroutine()
    {
        float percent = 1;
        yield return new WaitForSeconds(fadeDelay);
        while (percent > 0)
        {
            percent -= Time.deltaTime * fadeSpeed;
            messageText.color = new Color(initialColour.r, initialColour.g, initialColour.b, percent);
            yield return null;
        }
        currentMessage = null;
        yield return null;
    }
}
