using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorController : MonoBehaviour
{
    public float frozenPercent = 0f;
    public float freezeSpeed = 0.01f;
    public float eyesOpenPercent = 1f;
    public float eyesClosedSpeed = 2f;
    public Material eyesClosedMaterial;

    public static DirectorController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frozenPercent += freezeSpeed * Time.deltaTime;
        if (Input.GetButton("CloseEyes"))
        {
            eyesOpenPercent -= eyesClosedSpeed * Time.deltaTime;
            if (eyesOpenPercent < 0) eyesOpenPercent = 0;
        }
        else
        {
            eyesOpenPercent += eyesClosedSpeed * Time.deltaTime;
            if (eyesOpenPercent > 1) eyesOpenPercent = 1;
        }
        eyesClosedMaterial.SetFloat("Percent_Value", eyesOpenPercent);
    }
}
