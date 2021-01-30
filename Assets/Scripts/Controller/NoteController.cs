using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    public static NoteController instance;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Text;
    public Material noteMaterial;
    public Transform noteTransform;

    private Color titleStartColour;
    private Color textStartColour;

    private void Start()
    {
        if (instance == null) instance = this;
        titleStartColour = Title.color;
        textStartColour = Text.color;
    }

    private void Update()
    {
        
    }

    public void DisplayNote(Note note)
    {
        noteMaterial.SetFloat("Dissolve_Value", 1);
        Title.text = note.Title;
        Text.text = note.Text;
        Title.color = titleStartColour;
        Text.color = textStartColour;
        noteTransform.gameObject.SetActive(true);
    }

    public void BurnNote()
    {
        StartCoroutine(BurnNoteCoroutine());
    }

    IEnumerator BurnNoteCoroutine()
    {
        float percent = 1;
        noteMaterial.SetFloat("Dissolve_Value", 1);
        while (percent > 0)
        {
            percent -= Time.deltaTime;
            if (percent < 0) percent = 0;
            noteMaterial.SetFloat("Dissolve_Value", percent);
            Title.color = new Color(titleStartColour.r, titleStartColour.g, titleStartColour.b, percent);
            Text.color = new Color(textStartColour.r, textStartColour.g, textStartColour.b, percent);
            yield return null;
        }
        noteTransform.gameObject.SetActive(false);
        yield return null;
    }
}
