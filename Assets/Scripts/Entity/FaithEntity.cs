using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaithEntity : MonoBehaviour
{
    MeshCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DirectorController.instance.eyesOpenPercent < 0.1f)
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }
}
