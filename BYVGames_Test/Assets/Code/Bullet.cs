using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //---Overhead---
    private float bulletSpeed = 250f;

    void Start()
    {
       
    }

    void Update()
    {
      
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("STOP");
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Debug.Log("BAng");
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.AddForce(transform.right * bulletSpeed);
    }
}
