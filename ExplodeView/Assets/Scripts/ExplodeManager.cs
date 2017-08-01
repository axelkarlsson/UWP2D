using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeManager : MonoBehaviour
{
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
        GetComponentInChildren<ExplodeObject>().ExplodeView(false);
        transform.position = Camera.main.transform.position;
        GetComponentInChildren<ExplodeObject>().SetDefaultPosition();
    }
}
