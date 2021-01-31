using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public Sprite introSprite;
    public Sprite winSprite;
    public Transform credits;
    public Transform panel;

    public enum Cutscene
    {
        Intro,
        Win
    }

    public Cutscene cutscene;

    // Start is called before the first frame update
    void Start()
    {
        if (cutscene == Cutscene.Intro)
        {
            StartCoroutine(Intro());
        }
        else
        {
            StartCoroutine(Win());
        }
        
    }

    IEnumerator Win()
    {
        credits.gameObject.SetActive(false);
        image.sprite = winSprite;
        image.enabled = true;
        text.text = "I escape the hell of my own creation";
        yield return new WaitForSecondsRealtime(3f);
        text.text = "It is not the end of my struggles";
        yield return new WaitForSecondsRealtime(3f);
        text.text = "But now I can count on myself as an ally";
        yield return new WaitForSecondsRealtime(3f);
        text.text = "Above all, that brings me hope";
        yield return new WaitForSecondsRealtime(3f);
        text.enabled = false;
        image.enabled = false;
        credits.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(15f);
        SceneController.instance.MainMenu();
    }

    IEnumerator Intro()
    {
        image.sprite = null;
        image.enabled = false;
        text.text = "Press any key to continue.";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        text.text = "I awake in a room";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        text.text = "A chill bites to my bones";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        text.text = "I don't remember anything";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        image.sprite = introSprite;
        image.enabled = true;
        text.text = "A fireplace is before me";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        text.text = "It brings comfort, but not warmth";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        text.text = "Perhaps I should stay...";
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.3f);
        SceneController.instance.StartGame();

    }
}
