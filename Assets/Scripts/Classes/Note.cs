using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Note")]
public class Note : ScriptableObject
{
    public Guid Id = Guid.NewGuid();
    public string Title;
    public string Text;
}
