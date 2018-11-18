using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SwitchCannon : MonoBehaviour
{
    private GameObject cannonHorizonatal;
    private GameObject cannonVerical;
    private GameObject cannonShoot;
    private GameObject cannonSwitch;
    
    void Start ()
    {
        cannonHorizonatal = GameObject.Find("CannonHorizontal");
        cannonVerical = GameObject.Find("CannonVertical");
        cannonShoot = GameObject.Find("CannonShoot");
        cannonSwitch = GameObject.Find("SwitchCannon");
        
        cannonHorizonatal.SetActive(false);
        cannonVerical.SetActive(false);
        cannonShoot.SetActive(false);
    }

    void SetDefault()
    {
        
    }
    
    // Update is called once per frame
    void Update () {
        if (CrossPlatformInputManager.GetButtonDown("SwitchCannon"))
        {
                Switch();				
         
        }
    }

    public void Switch()
    {
            //Debug.Log("Changing cannon from : " + Cannons.cannonSelected);
            if (Cannons.cannonSelected != 0)
            {
                GameObject.Find("Cannon" + Cannons.cannonSelected).GetComponentInChildren<LaunchArcScript>().DeleteArc();
            }
            Cannons.cannonSelected = (Cannons.cannonSelected + 1) % 5;
            //Debug.Log("Changing cannon to : " + Cannons.cannonSelected);
            if (Cannons.cannonSelected == 0)
            {
                cannonHorizonatal.SetActive(false);
                cannonVerical.SetActive(false);
                cannonShoot.SetActive(false);
                cannonSwitch.GetComponentInChildren<Text>().text = "Select cannon";
            }
            else
            {
                cannonHorizonatal.SetActive(true);
                cannonVerical.SetActive(true);
                cannonShoot.SetActive(true);
                cannonSwitch.GetComponentInChildren<Text>().text = "Cannon" + Cannons.cannonSelected;
                float val = cannonVerical.GetComponent<Slider>().value;
                cannonVerical.GetComponent<SliderScript>().OnVerAngChange(val);
            }
    }

}
