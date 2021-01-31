using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAppearDissapearController : MonoBehaviour
{
    public DirectorController.Flags flag;
    public bool appearOnFlag = true;
    public Transform child;

    // Update is called once per frame
    void Update()
    {
        bool flagComplete = DirectorController.instance.IsFlagCompleted(flag);
        bool conditionMet = appearOnFlag ? flagComplete : !flagComplete;

        if (conditionMet)
        {
            child.gameObject.SetActive(true);
        }
        else
        {
            child.gameObject.SetActive(false);
        }

    }
}
