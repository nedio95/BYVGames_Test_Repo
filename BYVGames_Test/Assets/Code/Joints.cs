using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joints : MonoBehaviour {

    //----Overhead---
    public int DamageToDeal;

    private bool m_canBeDamaged = true;

    public void DestroyJoint()
    {
        Destroy(gameObject.GetComponent<HingeJoint2D>());
        transform.parent = null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!m_canBeDamaged) 
            return;

        Transform tcol = transform.root;
        GameObject rootObj = tcol.gameObject;
        
        GameObject obj = col.gameObject;
        if (obj.name == "Bullet(Clone)")
        {
            rootObj.GetComponent<PlayerController>().GotHit(DamageToDeal);
        }
        
    }
}
