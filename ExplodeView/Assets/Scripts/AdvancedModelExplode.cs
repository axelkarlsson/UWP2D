using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedModelExplode : ExplodeObject
{

    public override Vector3 ExplodePartPos(Transform t)
    {
        switch (t.name)
        {
            case "Explode1":
                return new Vector3(0, 0.2f, 0) + t.position; 
            case "Explode2":
                return new Vector3(0, 0.2f, 0) + t.position; 
            case "Base":
                return new Vector3(0, 0, 0) + t.position;
            case "NonExplode1":
                return new Vector3(0, 0.2f, 0) + t.position;
            case "NonExplode2":
                return new Vector3(0, 0.2f, 0) + t.position;
            case "Nonexplode3":
                return new Vector3(0, 0.2f, 0) + t.position;
            default:
                return new Vector3(0, 0, 0) + t.position;

        }

    }
}
