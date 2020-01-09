using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    // Setup    
    static private float m_bulletSpeed = 250f;

    // Unity Editor Setup
    public AudioClip m_snd_gunshot;
    public AudioClip m_snd_bulletCollision;

    //---Overhead---
    private bool m_isLethal = true;
    private int m_ownedPlayerNum;
    private AudioSource m_audSrs;
    

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = col.gameObject;
        if (obj.name == "Bullet(Clone)")
        {
            //When a bullet hits another bullet it becomse non-lethal and gets activates its gravity
            m_isLethal = false;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            m_audSrs.PlayOneShot(m_snd_bulletCollision, 0.7f); // I could not find a nice metal-on-metal sound
            return;
        }
        else
        {
            DeactivateThis();
        }
    }

    void OnEnable()
    {   
        //Play gunshot sound; More efficient to be on the gun
        // Reset state to "This bullet just got shot out of a gun"
        if(!m_audSrs) m_audSrs = GetComponent<AudioSource>();
        m_audSrs.PlayOneShot(m_snd_gunshot, 0.7f);
        m_isLethal = true;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.AddForce(transform.right * m_bulletSpeed);
        m_ownedPlayerNum = 0;
        if (transform.position.x > 0) m_ownedPlayerNum = 1;
    }

    void DeactivateThis()
    {
        // return bullet to bullet pool for later use
        gameObject.SetActive(false);
    }

    public bool IsBulletLethal()
    {
        // For other objects to check if this bullet is lethal
        return m_isLethal;
    }

    public int GetOwningPlayer()
    {
        // for other functions ot check which player this belongs to
        return m_ownedPlayerNum;
    }
    
}
