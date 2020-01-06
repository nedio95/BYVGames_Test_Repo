using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joints : MonoBehaviour {

    public void DestroyJoint()
    {
        Destroy(gameObject.GetComponent<HingeJoint2D>());
        transform.parent = null;
    }

}
