using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if NETFX_CORE
using Windows.ApplicationModel.Core;
#endif

public class ExplodeManager : MonoBehaviour
{
    private bool updateNextFrame;
    private Vector3 prevPos;
    public List<GameObject> explodeModels;
    // Use this for initialization
    private string currentObject;
    void Start()
    {
        string args = UnityEngine.WSA.Application.arguments;
        if (args.Contains("Uri="))
        {
            string objectName = args.Substring(args.IndexOf('/') + 2).ToLower();
            objectName = objectName.Remove(objectName.Length - 1);
            UpdateObject(objectName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (updateNextFrame)
        {
            if (Camera.main.transform.position != prevPos)
            {
                GetComponentInChildren<ExplodeObject>().ExplodeView(false);
                Vector3 dir = Camera.main.transform.forward;
                dir.y = 0;
                dir = Camera.main.transform.position + dir.normalized * 3.5f;
                transform.position = dir;
                GetComponentInChildren<ExplodeObject>().SetDefaultPosition();
                updateNextFrame = false;
            }
        }
        prevPos = Camera.main.transform.position;
    }

    public void UpdateObject(string objectName)
    {
        GameObject.FindGameObjectWithTag("NameShow").GetComponent<TextMesh>().text = objectName;
        if (currentObject != objectName)
        { 
            currentObject = objectName;
            DeleteModels();
            LoadModel(ModelType(objectName));
        }

        updateNextFrame = true;

    }

    private System.Random rnd = new System.Random();
    private string ModelType(string objectName)
    {
        switch (rnd.Next(2))
        {
            case 1:
                return "head";
            case 2:
                return "body";
            case 0:
            default:
                return "full";
        }
    }

    private void DeleteModels()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void LoadModel(string model)
    {
        switch (model)
        {
            case "head":
                GameObject head = GameObject.Instantiate(explodeModels[0], transform) as GameObject;
                head.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "body":
                GameObject body = GameObject.Instantiate(explodeModels[1], transform) as GameObject;
                body.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case "full":
            default:
                GameObject full = GameObject.Instantiate(explodeModels[2], transform) as GameObject;
                full.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
        }
    }

    public void UpdatePosition()
    {
        updateNextFrame = true;
    }
}
