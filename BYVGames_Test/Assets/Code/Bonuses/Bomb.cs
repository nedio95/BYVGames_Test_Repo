using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BonusesScript 
{
    // SETUP
    private float m_maxTime = 5f;
    private float m_currentTime = 5f;

    // Unity Editor Setup
    public GameObject TimeDisplay;
    public GameObject pref_explosion;

    // Overhead
    private bool m_isBombActive = false;

    // Resets bonus to spawn state
    public override void ResetBonus()
    {
        base.ResetBonus();
        m_isBombActive = false;
        m_currentTime = m_maxTime;
    }

    public override void DeactivateBonus()
    {
        base.DeactivateBonus();
        // Make an explosion when... well... exploding
        Instantiate(pref_explosion, transform.position, Quaternion.identity);
        m_isBombActive = false;
    }
    
    // Updates the text element explosion timer
    void UpdateTimer()
    {
        TimeDisplay.GetComponent<TextMesh>().text = m_currentTime.ToString("F2");
    }
	
	// Update is called once per frame
	void Update () 
    {
        // Tic-toc-boom; Countdown when hte bomb is activated
        if (m_isBombActive) m_currentTime -= Time.deltaTime;
        UpdateTimer();
        if (m_currentTime < 0f)
        {
            //explode
            DeactivateBonus();
        }
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!m_isBombActive) m_isBombActive = true;
        //Check if the collider was a player and explode
    }
}
