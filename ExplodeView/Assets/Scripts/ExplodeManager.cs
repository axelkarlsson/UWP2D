using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeManager : MonoBehaviour
{
    private bool needUpdate = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePosition()
    {
        transform.position = Camera.main.transform.position;
        needUpdate = true;
    }

    /*
     AppCallbacks.Instance.InvokeOnAppThread(new AppCallbackItem(() =>
                {
                    testFunc(args);
                }
                ), false);
                */
}
