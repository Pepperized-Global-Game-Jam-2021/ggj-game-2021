using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Text;
    public Material noteMaterial;
    public Transform noteTransform;

    private Color titleStartColour;
    private Color textStartColour;

    private void Start()
    {

        titleStartColour = Title.color;
        textStartColour = Text.color;
    }

    private void Update()
    {
        
    }

    public void DisplayNote(Note note)
    {
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
        while (percent > 0)
        {
            percent -= Time.deltaTime;
            noteMaterial.SetFloat("Dissolve_Value", percent);
            Title.color = new Color(titleStartColour.r, titleStartColour.g, titleStartColour.b, percent);
            Text.color = new Color(textStartColour.r, textStartColour.g, textStartColour.b, percent);
            yield return null;
        }
        noteTransform.gameObject.SetActive(false);
        yield return null;
    }
}
