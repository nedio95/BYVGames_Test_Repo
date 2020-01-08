using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{

    //---Overhead---
    static private float m_bulletSpeed = 250f;
    private bool m_isLethal = true;
    private int m_ownedPlayerNum;

    private AudioSource m_audSrs;
    public AudioClip m_snd_gunshot;
    public AudioClip m_snd_bulletCollision;

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = col.gameObject;
        if (obj.name == "Bullet(Clone)")
        {
            m_isLethal = false;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            m_audSrs.PlayOneShot(m_snd_bulletCollision, 0.7f);
            return;
        }
        else
        {
            DeactivateThis();
        }
    }

    void OnEnable()
    {
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
        gameObject.SetActive(false);
    }

    public bool IsBulletLethal()
    {
        return m_isLethal;
    }

    public int GetOwningPlayer()
    {
        return m_ownedPlayerNum;
    }
    
}
