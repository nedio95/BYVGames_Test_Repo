using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : Particles
{
	void Start () 
    {
        m_destroyAfter = 1f;
        DestroyMe();
	}
}
