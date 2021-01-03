using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    public float secondsUntilRemoval;

	// Update is called once per frame
	void Update () {
        secondsUntilRemoval -= Time.deltaTime;

        if (secondsUntilRemoval <= 0)
            Destroy(this.gameObject);
	}
}
