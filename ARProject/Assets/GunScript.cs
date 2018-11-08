using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class GunScript : MonoBehaviour {

    public float Damage = 2f;
    public float Range = 100f;

    public ParticleSystem muzzleFlash;
    public AudioSource gunshot;



    private Transform findParentWithTag(string tagname,Transform obj){

        Transform parent = obj.transform.parent;

        while(parent!=null){
            if(parent.tag == tagname){
                return parent;
            }
            parent = parent.transform.parent;
        }

        return null;
    }

    private GamerController getCurrentObjectGameController()
    {
        GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
        return gameController;
    }

    private bool canFireCheck(){
        GamerController mainGameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();

        // Now we need to get the parent of the gun -> whether the parent is player 1 or player 2
        string match1 = findParentWithTag("Player", this.transform).name;

        // This gets the current charater name invoking the script
        string match2 = mainGameController.getCurrentCharacter().transform.name;

        // Fire only when both are originating from the same object
        if (match1 != null && match1 == match2)
            return true;

        return false;

    }
	
	// Update is called once per frame
	void Update () {

        if(CrossPlatformInputManager.GetButton("Fire")){

            // Fire only when both are originating from the same object
            if (canFireCheck())
            {
                GamerController current = getCurrentObjectGameController();
                Actions currentActions = current.getCurrentActions();
                currentActions.Attack();
                //Debug.Log("Fire button pressed");
                Shoot();
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        muzzleFlash.Play();
        AudioClip clip = gunshot.clip;
        gunshot.PlayOneShot(clip);

        GamerController mainGameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();

        mainGameController.UpdateEnergy(0.5f);

        // This condition is true only when we have hit something with our ray
        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, Range))
        {
            //Debug.Log(hit.transform.name);


            // This gets the action component of the player if the raycast hit a player
            Actions character_actions = hit.transform.GetComponent<Actions>();

            // Character actions will be null if the player was not hit
            if (character_actions!=null){

                // This causes damage 
                character_actions.Damage();
                Dictionary<string, GameObject> healthDict = mainGameController.getCharacterHealthDict();
                float health = float.Parse(healthDict[hit.transform.name].GetComponent<UnityEngine.UI.Text>().text);
                health -= 2f;

                if(health>0){
                    healthDict[hit.transform.name].GetComponent<UnityEngine.UI.Text>().text = (health).ToString();
                }
                else{
                    health = 0;
                    healthDict[hit.transform.name].GetComponent<UnityEngine.UI.Text>().text = (health).ToString();
                    //Debug.Log("character actions ");
                    character_actions.Death();
                }
            }
        }
        else
        {
            //Debug.Log("We didn't hit anything but we fired !!!");
        }
    }
}
