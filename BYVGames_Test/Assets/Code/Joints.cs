using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joints : MonoBehaviour {

    //----Overhead---
    public int DamageToDeal;

    private bool m_canBeDamaged = true;
    private bool isJoint = false;
    private Vector3 m_jointAnchor;


    void Start()
    {
        if (gameObject.GetComponent<HingeJoint2D>())
        {
            isJoint = true;
            m_jointAnchor = gameObject.GetComponent<HingeJoint2D>().anchor;
        }
    }
    void Update()
    {
        if(isJoint)
            gameObject.GetComponent<HingeJoint2D>().anchor = m_jointAnchor;
    }   

    //This is for explosions
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
            if (!obj.GetComponent<Bullet>().IsBulletLethal()) 
                return;
            rootObj.GetComponent<PlayerController>().GotHit(DamageToDeal, gameObject.transform);
        }
        
    }
}
