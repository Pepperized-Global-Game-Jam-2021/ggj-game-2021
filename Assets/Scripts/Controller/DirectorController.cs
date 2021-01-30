using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorController : MonoBehaviour
{
    public float frozenPercent = 0f;
    public float freezeSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frozenPercent += freezeSpeed * Time.deltaTime;
    }
}
