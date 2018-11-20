using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlScript: MonoBehaviour {

    public GameObject HorizontalSlider;
    public GameObject VerticalSlider;
    public GameObject SwitchCannons;

    public void ShowSliderControls()
    {
        HorizontalSlider.SetActive(true);
        VerticalSlider.SetActive(true);
    }

    public void HideSliderControl()
    {
        HorizontalSlider.SetActive(false);
        VerticalSlider.SetActive(false);
    }

    public void ShowCannonSwitch(){
        SwitchCannons.SetActive(true);
    }

    public void HideCannonSwitch(){
        SwitchCannons.SetActive(false);
    }

    public void Toggle_Attack_Type(bool value){
        if(value){
            ShowSliderControls();
        }else{
            HideSliderControl();
        }
    }

    public void Toggle_Weapon_Type(bool value){
        if(value){
            ShowCannonSwitch();
        }else{
            HideCannonSwitch();
        }
    }
}
