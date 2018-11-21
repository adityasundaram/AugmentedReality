using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion: MonoBehaviour {

	public GameObject explosion;
    
	void OnCollisionEnter(Collision collision)
	{
		GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
		Destroy(exp, 2);
	}
}
