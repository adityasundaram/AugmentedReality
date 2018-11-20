using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

public class SliderScript : MonoBehaviour {

    public GameObject toggle_weapon_type;

	
	public void OnVerAngChange(float val)
	{
        Debug.Log(toggle_weapon_type);
        Debug.Log(toggle_weapon_type.GetComponent<Toggle>());
        Debug.Log(toggle_weapon_type.GetComponent<Toggle>().isOn);
        if (toggle_weapon_type.GetComponent<Toggle>().isOn){

            Debug.Log("This is working fine - cannon");

            Debug.Log(Cannons.cannonSelected);
            //change cannon angle
            GameObject cannon = GameObject.Find("Cannon" + Cannons.cannonSelected);

            Debug.Log(cannon);
            cannon.transform.localEulerAngles = new Vector3(360.0f - (45 * val), cannon.transform.localEulerAngles.y, cannon.transform.localEulerAngles.z);

            //change projectile path angle
            GameObject projectilePath = cannon.transform.GetChild(1).gameObject;
            projectilePath.transform.localEulerAngles = new Vector3(45 * val, projectilePath.transform.localEulerAngles.y, projectilePath.transform.localEulerAngles.z);
            cannon.GetComponentInChildren<LaunchArcScript>().RenderArc(45 * val);
        }else{

            Debug.Log("This is working fine - grenade");

            GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
            GameObject currentCharacter = gameController.GetCurrentCharacterProperties().charObject;

            //change projectile path angle
            GameObject projectilePath = currentCharacter.transform.Find("ProjectilePath").gameObject;
            projectilePath.transform.localEulerAngles = new Vector3(-90 * val, projectilePath.transform.localEulerAngles.y, projectilePath.transform.localEulerAngles.z);
            projectilePath.GetComponent<LaunchArcScript>().RenderArc(-90 * val);
        }
		  
	}

	public void onHorAngChange(float val)
	{
        if (toggle_weapon_type.GetComponent<Toggle>().isOn)
        {
            GameObject cannon = GameObject.Find("Cannon" + Cannons.cannonSelected);
            int initialAngle = Cannons.cannonAngles[Cannons.cannonSelected - 1];
            float ang = initialAngle + (45 * val);
            cannon.transform.localEulerAngles = new Vector3(cannon.transform.localEulerAngles.x, ang, cannon.transform.localEulerAngles.z);
        }
        else{
            GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
            GameObject currentCharacter = gameController.GetCurrentCharacterProperties().charObject;

            GameObject projectilePath = currentCharacter.transform.Find("ProjectilePath").gameObject;

            float ang = projectilePath.transform.localEulerAngles.y;
            ang = 90 * val;

            projectilePath.transform.localEulerAngles = new Vector3(projectilePath.transform.localEulerAngles.x, ang, projectilePath.transform.localEulerAngles.z);
        }
	}
}
