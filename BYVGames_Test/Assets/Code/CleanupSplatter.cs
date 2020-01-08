using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupSplatter : MonoBehaviour 
{
    
	// Not pooling particle systems as with the scope of the test and the expected occurence of blood splatters
    // not pooling them (like the bullets) should not cause major issues
    // In case that I am expecting a major amount of objects to be required and then made redundant fast in small amounts of time
    // as example in strategy game where hundreds of units shoot at hundreds of other units then the particles would require much better management
    //Alas this would do for this test scope

	void Start () 
    {
        Destroy(gameObject, 1f);
	}
}
