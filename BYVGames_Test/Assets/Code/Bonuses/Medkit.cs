using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : BonusesScript 
{
    // SETUP
    private int m_HP = 5;
    private int m_maxHP = 5;
    private int m_healAmount = 3;

    //Unity Editor Setup
    public GameObject m_HPtextMesh;
    
    void UpdateHealthDisplay()
    {
        // Updates the text on the health display
        m_HPtextMesh.GetComponent<TextMesh>().text = m_HP.ToString();
    }

    public override void ResetBonus()
    {
        //Resets the bonus to spawn states
        base.ResetBonus();       
        m_HP = m_maxHP;
    }

    public override void DeactivateBonus()
    {
        //Deactivates the bonus
        base.DeactivateBonus();
        m_HP = m_maxHP;
        UpdateHealthDisplay();
        gameObject.SetActive(false);
    }

    // Collision detection can be inherited too ?
    void OnCollisionEnter2D(Collision2D col)
    {
        if (TheGameManager.GetComponent<GameManager>().IsGameOver()) return;

        //Manages health of the medkit bonus
        GameObject obj = col.gameObject;
        if (obj.name == BulletName)
        {
            if (obj.GetComponent<Bullet>().IsBulletLethal())
            {
                m_HP--;
                UpdateHealthDisplay();
                if (m_HP < 1)
                {
                    int owner = obj.GetComponent<Bullet>().GetOwningPlayer();
                    //This deals negative damage... 
                    //Should make a separate function that processes healing but this works for the moment
                    TheGameManager.GetComponent<GameManager>().PlayerGotShot(-m_healAmount, owner);
                    DeactivateBonus();
                }
                
            }
        }
    }
}
