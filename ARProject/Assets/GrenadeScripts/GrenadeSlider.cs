using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSlider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void OnVerAngChange(float val)
	{
		GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
		GameObject currentCharacter = gameController.GetCurrentCharacterProperties().charObject;

		//change Grenade throw angle
		GameObject grenade = currentCharacter.transform.Find("Grenade").gameObject;
		grenade.transform.localEulerAngles = new Vector3(-45 * val, grenade.transform.localEulerAngles.y, grenade.transform.localEulerAngles.z);
		
		//Render arc
		currentCharacter.transform.Find("ProjectilePath").GetComponent<LaunchArcScript>().RenderArc(45 * val);
	}
}
