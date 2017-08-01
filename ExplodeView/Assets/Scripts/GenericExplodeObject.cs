using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericExplodeObject : ExplodeObject {

    public override Vector3 ExplodePartPos(Transform t)
    {
        Vector3 direction = (t.position - t.parent.position);
        return t.position + (direction * (float)(System.Math.Log(2 + direction.magnitude)));
    }
         
}
