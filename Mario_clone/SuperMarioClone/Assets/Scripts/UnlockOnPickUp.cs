using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockOnPickUp : MonoBehaviour {

    [GiveTag]
    public string[] InteractWith = new string[] { };

    [GiveTag]
    public string[] stopMovementAt = new string[] { };



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
