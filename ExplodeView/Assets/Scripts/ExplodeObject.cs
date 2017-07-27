using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

[RequireComponent(typeof(Rigidbody))]
public abstract class ExplodeObject : MonoBehaviour, IInputClickHandler {
    private Dictionary<Transform, Vector3> defaultPositions = new Dictionary<Transform, Vector3>();
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
		foreach(Transform t in transform)
        {
            defaultPositions[t] = t.position; 
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void ExplodeView()
    {
        //Reset to default position if exploded
        if (explodedActive)
        {
            foreach(Transform t in defaultPositions.Keys)
            {
                t.position = defaultPositions[t];
                foreach (ExplodeObject e in t.GetComponentsInChildren<ExplodeObject>())
                {
                    e.ExplodeView(false);
                }
            }
        }
        else
        {

            foreach (Transform t in defaultPositions.Keys)
            {
                ExplodePart(t);
                foreach(ExplodeObject e in t.GetComponentsInChildren<ExplodeObject>())
                {
                    
                    e.ExplodeView(true);
                }
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

    public abstract void ExplodePart(Transform t);

	
	// Update is called once per frame
	void Update () {
		
	}
}
