using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEntity : MonoBehaviour
{
    public Note Note;
    public DirectorController.Flags pickupFlag;

    private void Start()
    {
        if (Note.name == "TestNote")
        {
            Debug.LogWarning("Test note still in level!");
        }
    }
}
