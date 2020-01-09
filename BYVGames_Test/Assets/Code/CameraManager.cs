using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    public float width = 16f;
    public float height = 9f;

    // Unity cameras are weird...
    void Start()
    {
        Camera.main.aspect = width / height;
    }
}
