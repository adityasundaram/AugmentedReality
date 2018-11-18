using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class SliderScript : MonoBehaviour {

	
	public void OnVerAngChange(float val)
	{
		//change cannon angle
		GameObject cannon = GameObject.Find("Cannon"+Cannons.cannonSelected);
		cannon.transform.localEulerAngles = new Vector3(360.0f - (45 * val),cannon.transform.localEulerAngles.y,cannon.transform.localEulerAngles.z);
				
		//change projectile path angle
		GameObject projectilePath = cannon.transform.GetChild(1).gameObject;
		projectilePath.transform.localEulerAngles = new Vector3(45 * val ,projectilePath.transform.localEulerAngles.y,projectilePath.transform.localEulerAngles.z);
		cannon.GetComponentInChildren<LaunchArcScript>().RenderArc(45*val);  
	}

	public void onHorAngChange(float val)
	{
		GameObject cannon = GameObject.Find("Cannon"+Cannons.cannonSelected);
		int initialAngle = Cannons.cannonAngles[Cannons.cannonSelected - 1];
		float ang = initialAngle + (45 * val);
		cannon.transform.localEulerAngles = new Vector3(cannon.transform.localEulerAngles.x, ang ,cannon.transform.localEulerAngles.z);
	}
}
