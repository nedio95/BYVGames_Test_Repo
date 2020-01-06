using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    //----Overhead----
    public GameObject pref_bullet;

    private List<GameObject> list_bullets = new List<GameObject>();

    public void Shoot(GameObject gun)
    {
        Vector3 pos = gun.transform.position;
        Quaternion rot = gun.transform.rotation;

        ActivateBullet(pos, rot);
    }
    void Update()
    {
        /*Mouse Testing 
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 1;
            pos = Camera.main.ScreenToWorldPoint(pos);

            ActivateBullet(pos, Quaternion.identity);
        }
        */
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

}
