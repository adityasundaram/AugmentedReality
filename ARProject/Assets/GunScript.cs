using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using Global;

public class GunScript : MonoBehaviour {

    public float Damage = 2f;
    public float Range = 100f;

    public ParticleSystem muzzleFlash;
    public AudioSource gunshot;


    private GamerController GetGameController()
    {
        GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
        return gameController;
    }


    private Transform FindParentWithTag(string tagname,Transform obj){

        Transform parent = obj.transform.parent;

        while(parent!=null){
            if(parent.tag == tagname){
                return parent;
            }
            parent = parent.transform.parent;
        }

        return null;
    }



    private bool canFireCheck(CharacterObject currentPlayer){

        // Now we need to get the parent of the gun -> whether the parent is player 1 or player 2
        string match1 = FindParentWithTag("Player", this.transform).name;

        // This gets the current charater name invoking the script
        string match2 = currentPlayer.charObject.transform.name;

        // Fire only when both are originating from the same object
        if (match1 != null && match1 == match2)
            return true;

        return false;

    }
	
	// Update is called once per frame
	void Update () {

        if(CrossPlatformInputManager.GetButton("Fire")){

            GamerController mainGameController = GetGameController();
            int currentPlayerIndex = mainGameController.GetCurrentPlayerIndex();

            CharacterObject currentObject = mainGameController.GetCharacterProperties(currentPlayerIndex);

            // Fire only when both are originating from the same object
            if (canFireCheck(currentObject))
            {
                currentObject.charActions.Attack();
                Shoot(mainGameController, currentObject, currentPlayerIndex, (currentPlayerIndex+1)%2);
            }
        }
    }

    void Shoot(GamerController mainGameController, CharacterObject currentPlayer, int currentPlayerIndex, int opponentIndex)
    {
        // This is the ray which will hit the target
        RaycastHit hit;

        // This is for the effect of shooting
        muzzleFlash.Play();
        AudioClip clip = gunshot.clip;
        gunshot.PlayOneShot(clip);

        // Updates the stamina of the player
        mainGameController.UpdateEnergy(0.5f);

        // This condition is true only when we have hit something with our ray
        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, Range))
        {
            //Debug.Log(hit.transform.name);


            // This gets the action component of the player if the raycast hit a player
            //Actions character_actions = hit.transform.GetComponent<Actions>();

            if (hit.transform.tag=="Player"){

                //character_actions.Damage();

                // This causes damage 
                mainGameController.TakeDamage(opponentIndex);
            }
        }
        else
        {
            //Debug.Log("We didn't hit anything but we fired !!!");
        }
    }
}
