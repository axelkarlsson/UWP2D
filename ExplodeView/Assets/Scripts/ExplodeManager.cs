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
        else
        {
            UpdateObject("test"); //Remove this before release
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
            LoadModel(objectName);
        }

        updateNextFrame = true;

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
        GameObject newObj = GameObject.Instantiate(explodeModels[0], transform) as GameObject;
        AddColliders(newObj);
    }

    void AddColliders(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer rend in renderers)
        {
            if(rend.GetComponent<MeshCollider>() == null)
            {
                rend.gameObject.AddComponent<MeshCollider>();
            }
            else
            {
                rend.GetComponent<MeshCollider>().enabled = true;
            }
                  
        }
    }

    public void UpdatePosition()
    {
        updateNextFrame = true;
    }
}
