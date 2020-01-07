using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    //----Overhead----
    public GameObject pref_bullet;
    public GameObject m_player1;
    public GameObject m_player2;
    private int[] PlayerHealth = new int[2] { 5, 5 };
    

    private List<GameObject> list_bullets = new List<GameObject>();

    void Start()
    {
    }

    public void Shoot(GameObject gun)
    {
        Vector3 pos = gun.transform.position;
        Quaternion rot = gun.transform.rotation;

        ActivateBullet(pos, rot);
    }
    void Update()
    {
        ///Mouse Testing 
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (pos.x < 0)
            {
                //JumpLeft
                m_player1.GetComponent<PlayerController>().Jump();
                
            }
            else    
            {
                m_player2.GetComponent<PlayerController>().Jump();
                //JumpRight
            }
        }
        
        //Touch Input Manager
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < 0)
            {
                ActivateBullet(touch.position, Quaternion.identity);
            }

        }
    }

    //---BULLET MANAGEMENT----
    

    public void BulletNeeded(Vector3 pos, Quaternion rot)
    {
        ActivateBullet(pos, rot);
    }
    private void ActivateBullet(Vector3 pos, Quaternion rot)
    {
        GameObject activateThisBullet = null;

        if (list_bullets.Count == 0) CreateNewBullet();

        bool needNewBullet = true;
        for (int i = 0; i < list_bullets.Count; i++)
        {
            if (list_bullets[i].active == false)
            {
                activateThisBullet = list_bullets[i];
                needNewBullet = false;

            }
        }

        if (needNewBullet == true)
            activateThisBullet = CreateNewBullet();

        activateThisBullet.transform.position = pos;
        activateThisBullet.transform.rotation = rot;
        activateThisBullet.SetActive(true);
    }

    private GameObject CreateNewBullet()
    {
        GameObject obj = Instantiate(pref_bullet, new Vector3(0, 0, 0), Quaternion.identity);
        obj.SetActive(false);
        list_bullets.Add(obj);
        return obj;
    }

    public void PlayerGotShot(int dmg, int num)
    {
        Debug.Log("Got it so far");
        PlayerHealth[num] -= dmg;
        if (PlayerHealth[num] < 1)
        {
            //Player num is dead
            Debug.Log("Murder call the police !");
        }
    }

}
