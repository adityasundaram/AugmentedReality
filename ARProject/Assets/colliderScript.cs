using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderScript : MonoBehaviour {

    // Trigger events - this basically collects th gun orb which allows the player get the power to use the gun
    private void OnTriggerEnter(Collider col){
        if(col.name=="FlareMobile1" || col.name=="FlareMobile2"){
            GamerController mainGameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();  
            var allowedWeaponIndex = mainGameController.getAllowedWeaponIndex();
            allowedWeaponIndex[this.name].Add(1);
            allowedWeaponIndex[this.name].Add(2);
            Destroy(col.gameObject);
        }else if (col.name=="FlareMobile3"){
            GamerController mainGameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
            Dictionary<string, GameObject> healthDict = mainGameController.getCharacterHealthDict();
            //Debug.Log("Collider is " + mainGameController.getCurrentCharacter().name);
            float health = float.Parse(healthDict[mainGameController.getCurrentCharacter().name].GetComponent<UnityEngine.UI.Text>().text);

            //Debug.Log("SciFiEngineer_1"+healthDict["SciFiEngineer_1"].GetComponent<UnityEngine.UI.Text>().text);
            //Debug.Log("SciFiEngineer_2" + healthDict["SciFiEngineer_2"].GetComponent<UnityEngine.UI.Text>().text);
            health += 20.0f;
            //Debug.Log("Health before is " + health.ToString());
            health = Mathf.Min(health, 100);
            healthDict[mainGameController.getCurrentCharacter().name].GetComponent<UnityEngine.UI.Text>().text = (health).ToString();
            Destroy(col.gameObject);
        }
    }
}
