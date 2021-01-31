using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DirectorController : MonoBehaviour
{
    public enum Flags
    {
        None,
        FirstNoteTaken,
        SecondNoteTaken,
        Collectathon,
        CollectathonComplete,
        SpatialPuzzle,
        Win
    }

    public float frozenPercent = 0f;
    public float freezeSpeed = 0.01f;
    public float eyesOpenPercent = 1f;
    public float eyesClosedSpeed = 2f;
    public Material eyesClosedMaterial;
    public AnimationCurve frozenEyeCloseCurve;
    public bool isFreezingPaused = false;
    public Image frozenVingette;

    public static DirectorController instance;

    List<Flags> flagsCompleted = new List<Flags>();

    public void CompleteFlag(Flags flag)
    {
        flagsCompleted.Add(flag);

        if (FlagAmount(Flags.Collectathon) > 2 && !IsFlagCompleted(Flags.CollectathonComplete))
        {
            CompleteFlag(Flags.CollectathonComplete);
        }
    }

    public bool IsFlagCompleted(Flags flag)
    {
        return flagsCompleted.Contains(flag);
    }

    public int FlagAmount(Flags flag)
    {
        return flagsCompleted.Count(x => x == Flags.Collectathon);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Game over");
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
        float vingetteOpacity = 0;

        if (frozenPercent > 0.8f)
        {
            eyesOpenMaxPercent = frozenEyeCloseCurve.Evaluate(Mathf.InverseLerp(1, 0.8f, frozenPercent));
        }
        if (frozenPercent > 0.6f)
        {
            vingetteOpacity = Mathf.InverseLerp(0.6f, 1f, frozenPercent);
        }

        frozenVingette.color = new Color(frozenVingette.color.r, frozenVingette.color.g, frozenVingette.color.b, vingetteOpacity);

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
