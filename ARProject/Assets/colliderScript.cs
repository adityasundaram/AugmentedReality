using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class colliderScript : MonoBehaviour {


    private GamerController GetGameController()
    {
        GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
        return gameController;
    }

    // Trigger events - this basically collects th gun orb which allows the player get the power to use the gun
    private void OnTriggerEnter(Collider col){

        GamerController mainGameController = GetGameController();
        CharacterObject currentPlayer = mainGameController.GetCurrentCharacterProperties();

        if (col.name=="FlareMobile1" || col.name=="FlareMobile2"){

            currentPlayer.WeaponAllowed = true;
            Destroy(col.gameObject);

        }else if (col.name=="FlareMobile3"){

            mainGameController.UpdateHealth(currentPlayer, 20f);
            Destroy(col.gameObject);
        }
    }
}
