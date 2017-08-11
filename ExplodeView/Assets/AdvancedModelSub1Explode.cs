using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedModelSub1Explode : ExplodeObject
{

    public override Vector3 ExplodePartPos(Transform t)
    {
        Debug.Log(t.name);
        switch (t.name)
        {
            case "Part1":
                return new Vector3(0, 0.2f, 0) + t.position;
            case "Part2":
                return new Vector3(0.2f, 0.2f, 0) + t.position;
            case "Part3":
                return new Vector3(0.4f, 0.2f, 0) + t.position;
            case "Part4":
                return new Vector3(0.6f, 0.2f, 0) + t.position;
            case "Part5":
                return new Vector3(0.8f, 0.2f, 0) + t.position;
            default:
                return new Vector3(0, 0, 0) + t.position;

        }

    }
}
