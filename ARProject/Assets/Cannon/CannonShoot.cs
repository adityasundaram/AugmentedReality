using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CannonShoot : MonoBehaviour
{
	public GameObject cannonBall;
	public AudioSource cannonSound;
	public ParticleSystem muzzleFlash;
	
	public float firePower;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("CannonShoot"))
		{
			GameObject cannon = gameObject.transform.parent.gameObject; 
			if (cannon.name.Equals("Cannon"+Cannons.cannonSelected))
			{
				ShootCannon();				
			}
		}
	}

	public void ShootCannon()
	{
		muzzleFlash.Play();
		cannonSound.Play();
		
		//delete arc
		//GameObject cannon = gameObject.transform.parent.gameObject;
		//cannon.GetComponentInChildren<LaunchArcScript>().DeleteArc();
		
		//shoot cannon
		GameObject cannonBallToShoot = Instantiate(cannonBall, transform.position, transform.rotation);
		cannonBallToShoot.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, firePower * 1.414f *0.145f));
	}

}
