using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using Global;

public class GrenadeScript : MonoBehaviour {


    public GameObject GrenadeObject;

    private GamerController GetGameController()
    {
        GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
        return gameController;
    }


    private Transform FindParentWithTag(string tagname, Transform obj)
    {

        Transform parent = obj.transform.parent;

        while (parent != null)
        {
            if (parent.tag == tagname)
            {
                return parent;
            }
            parent = parent.transform.parent;
        }

        return null;
    }

    private bool canFireCheck(CharacterObject currentPlayer)
    {

        // Now we need to get the parent of the gun -> whether the parent is player 1 or player 2
        string match1 = FindParentWithTag("Player", this.transform).name;

        // This gets the current charater name invoking the script
        string match2 = currentPlayer.charObject.transform.name;

        // Fire only when both are originating from the same object
        if (match1 != null && match1 == match2)
            return true;

        return false;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            GamerController mainGameController = GetGameController();
            int currentPlayerIndex = mainGameController.GetCurrentPlayerIndex();

            Debug.Log("Here we have index:"+currentPlayerIndex);

            CharacterObject currentObject = mainGameController.GetCharacterProperties(currentPlayerIndex);

            Debug.Log(currentObject);

            // Fire only when both are originating from the same object
            if (canFireCheck(currentObject))
            {
                currentObject.charActions.Attack();
                ThrowGrenade();
            }
        }

    }

    public void ThrowGrenade()
    {
        Debug.Log("Reached to throwing");

        Debug.Log(GrenadeObject);
        Debug.Log(transform.position);
        Debug.Log(transform.rotation);
        //shoot cannon
        GameObject grenadeObjectToThrow = Instantiate(GrenadeObject, transform.position, transform.rotation);
        grenadeObjectToThrow.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 16 * 1.414f * 0.145f));
    }
}
