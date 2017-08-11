using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedModelSub2Explode : ExplodeObject
{

    public override Vector3 ExplodePartPos(Transform t)
    {
        switch (t.name)
        {
            case "Part1_001":
                return new Vector3(0, 0.2f, 0) + t.position;
            case "Part2_001":
                return new Vector3(-0.2f, 0.2f, 0) + t.position;
            case "Part3_001":
                return new Vector3(-0.4f, 0.2f, 0) + t.position;
            case "Part4_001":
                return new Vector3(-0.6f, 0.2f, 0) + t.position;
            case "Part5_001":
                return new Vector3(-0.8f, 0.2f, 0) + t.position;
            default:
                return new Vector3(0, 0, 0) + t.position;

        }

    }
}
