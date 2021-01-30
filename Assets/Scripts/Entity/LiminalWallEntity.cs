using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiminalWallEntity : MonoBehaviour
{
    DirectorController director;
    MeshCollider col;

    private void Start()
    {
        director = DirectorController.instance;
        col = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        if (director.eyesOpenPercent < 0.1f)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
}
