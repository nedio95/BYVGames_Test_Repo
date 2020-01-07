using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{

    //---Overhead---
    static private float m_bulletSpeed = 250f;
    private bool m_isLethal = true;

    private AudioSource audSrs;

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = col.gameObject;
        if (obj.name == "Bullet(Clone)")
        {
            m_isLethal = false;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            return;
        }
        else
        {
            DeactivateThis();
        }
    }

    void OnEnable()
    {
        if(!audSrs)audSrs = GetComponent<AudioSource>();
        audSrs.Play();
        m_isLethal = true;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.AddForce(transform.right * m_bulletSpeed);
    }

    void DeactivateThis()
    {
        gameObject.SetActive(false);
    }

    
}
