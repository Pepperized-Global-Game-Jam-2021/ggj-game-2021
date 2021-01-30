using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uween;

public class DoorController : MonoBehaviour
{
    public Transform doorPivot;
    public bool isOpen = false;
    public DirectorController.Flags requiredFlag;

    public void OpenDoor(Vector3 playerPos)
    {
        Vector3 diff = transform.position - playerPos;
        bool posZ = diff.z > 0;
        TweenRY.Add(doorPivot.gameObject, 1f, posZ ? 90f : -90f).EaseOutQuad();
    }
}
