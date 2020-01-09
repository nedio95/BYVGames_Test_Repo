using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Particles {

    
    // Use this for initialization
    void Start () 
    {
        m_destroyAfter = 4f;
        DestroyMe();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = col.gameObject;
        obj = obj.transform.root.gameObject;
        //Player got exploded
        
    }
}
