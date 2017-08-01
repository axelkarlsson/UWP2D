using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

[RequireComponent(typeof(Rigidbody))]
public abstract class ExplodeObject : MonoBehaviour, IInputClickHandler {
    private Dictionary<Transform, Vector3> defaultPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> explodedPositions = new Dictionary<Transform, Vector3>();
    private bool explodedActive = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        var e = GetComponentsInParent<ExplodeObject>();
        if (e.Length > 1)
        {
            e[e.Length - 1].ExplodeView();
        }
        else
        {
            ExplodeView();
        }
    }

    // Use this for initialization
    void Start () {
        foreach (Transform t in transform)
        {
            defaultPositions[t] = t.position;
            explodedPositions[t] = ExplodePartPos(t);
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    //Updates the defaultpositions to their new values
    public void SetDefaultPosition()
    {
        foreach (Transform t in transform)
        {
            defaultPositions[t] = t.position;
            explodedPositions[t] = ExplodePartPos(t);
            var child = t.GetComponentInChildren<ExplodeObject>();
            if(child != null)
            {
                child.SetDefaultPosition();
            }
        }
    }


    public void ExplodeView()
    {

        foreach(Transform t in defaultPositions.Keys)
        {
            t.position = explodedActive ? defaultPositions[t] : explodedPositions[t];
            foreach (ExplodeObject e in t.GetComponentsInChildren<ExplodeObject>())
            {
                e.ExplodeView(!explodedActive);
            }
        }

        explodedActive = !explodedActive;
    }

    public void ExplodeView(bool state)
    {
        if(explodedActive != state)
        {
            ExplodeView();
        }
    }

    public abstract Vector3 ExplodePartPos(Transform t);

	
	// Update is called once per frame
	void Update () {
		
	}
}
