using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joints : MonoBehaviour {

    //Unity Editor Setup 
    public int DamageToDeal;

    // Overhead
    private bool m_canBeDamaged = true;
    private bool isJoint = false;
    private Vector3 m_jointAnchor;


    void Start()
    {
        //This fixes a very annoying bug with the joints
        if (gameObject.GetComponent<HingeJoint2D>())
        {
            isJoint = true;
            m_jointAnchor = gameObject.GetComponent<HingeJoint2D>().anchor;
        }
    }
    void Update()
    {
        //This fixes a very annoying bug with the joints
        if(isJoint)
            gameObject.GetComponent<HingeJoint2D>().anchor = m_jointAnchor;
    }   

    //This is for explosions
    public void DestroyJoint()
    {
        //This should be called when the player is exploded so the joints separate from the main body
        Destroy(gameObject.GetComponent<HingeJoint2D>());
        transform.parent = null;
    }

    //Get hit by a bullet, tell the mean bullet to the papa object
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
