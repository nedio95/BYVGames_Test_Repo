using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusesScript : MonoBehaviour 
{
    public GameObject TheGameManager;
    public string BulletName = "Bullet(Clone)";
    protected Vector3 m_bonusSpawnPos = new Vector3(0, 12f, 0);

    public virtual void DeactivateBonus()
    {
        TheGameManager.GetComponent<GameManager>().SetReadyForNewBonus();
        gameObject.SetActive(false);
    }

    public virtual void ResetBonus()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = m_bonusSpawnPos;
        gameObject.transform.rotation = Quaternion.identity;
    }
}
