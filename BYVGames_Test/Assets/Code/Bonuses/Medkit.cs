using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : BonusesScript 
{
    public GameObject TheGameManager;
    public GameObject m_textMesh;
    private int m_HP = 5;
    private int m_maxHP = 5;
    private int m_healAmount = 3;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update() 
    {
		
	}

    public override void DeactivateBonus()
    {
        m_HP = m_maxHP;
        m_textMesh.GetComponent<TextMesh>().text = m_HP.ToString();
        gameObject.SetActive(false);
    }

    // Collision detection can be inherited too ?
    void OnCollisionEnter2D(Collision2D col)
    {
        if (TheGameManager.GetComponent<GameManager>().IsGameOver()) return;

        GameObject obj = col.gameObject;
        if (obj.name == BulletName)
        {
            if (obj.GetComponent<Bullet>().IsBulletLethal())
            {
                m_HP--;
                m_textMesh.GetComponent<TextMesh>().text = m_HP.ToString();
                if (m_HP < 1)
                {
                    int owner = obj.GetComponent<Bullet>().GetOwningPlayer();
                    //This deals negative damage... 
                    //Should make a separate function that processes healing but this works
                    TheGameManager.GetComponent<GameManager>().PlayerGotShot(-m_healAmount, owner);
                    DeactivateBonus();
                }
                
            }
        }
    }
}
