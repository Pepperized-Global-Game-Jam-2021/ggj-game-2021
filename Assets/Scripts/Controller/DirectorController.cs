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
    public AnimationCurve frozenEyeCloseCurve;
    public bool isFreezingPaused = false;

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
        if (!isFreezingPaused) frozenPercent += freezeSpeed * Time.deltaTime;

        float eyesOpenMaxPercent = 1;

        if (frozenPercent > 0.8f)
        {
            eyesOpenMaxPercent = frozenEyeCloseCurve.Evaluate(Mathf.InverseLerp(1, 0.8f, frozenPercent));
        }

        if (Input.GetButton("CloseEyes"))
        {
            eyesOpenPercent -= eyesClosedSpeed * Time.deltaTime;
            if (eyesOpenPercent < 0) eyesOpenPercent = 0;
        }
        else
        {
            eyesOpenPercent += eyesClosedSpeed * Time.deltaTime;
            if (eyesOpenPercent > eyesOpenMaxPercent) eyesOpenPercent = eyesOpenMaxPercent;
        }
        eyesClosedMaterial.SetFloat("Percent_Value", eyesOpenPercent);

        if (frozenPercent > 1)
        {
            Debug.Log("ded");
        }
    }
}
