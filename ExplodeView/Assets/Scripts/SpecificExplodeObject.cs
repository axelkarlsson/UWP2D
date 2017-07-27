using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificExplodeObject : ExplodeObject
{

    public override void ExplodePart(Transform t)
    {
        Vector3 direction = (t.position - t.parent.position);
        direction.z = 0;
        direction.y = 0;
        t.position += (direction * (float)(System.Math.Log(2 + direction.magnitude)));
    }
}
