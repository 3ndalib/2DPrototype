using UnityEngine;
using System.Collections;

public class Kill : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Spike")
			Application.LoadLevel(Application.loadedLevel);
	}
}