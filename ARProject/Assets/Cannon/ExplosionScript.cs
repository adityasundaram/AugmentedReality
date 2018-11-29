using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public GameObject explosion;

    private float impactDistance = 0.30f;
    
    private GamerController GetGameController()
    {
        GamerController gameController = GameObject.FindWithTag("GameController").GetComponent<GamerController>();
        return gameController;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        GamerController gamerController = GetGameController();            
        List<CharacterObject> characterObjects = gamerController.GetAllCharacterObjects();
        
        //get contact point
        Vector3 collisionPosition = collision.contacts[0].point;
        //Debug.Log("collisionPosition" + collisionPosition);
        for (int i = 0; i <= 1; ++i)
        {
            Vector3 characterPosition = characterObjects[i].charObject.transform.position;
            //Debug.Log("Character "+ i +"position : "+characterPosition);
            float distance = Vector3.Distance(collisionPosition, characterPosition);
            //Debug.Log("Distance "+ i +"position : "+distance);
            if (distance < impactDistance)
            {
                int health = (int) ((1 - (distance / impactDistance)) * 40);
                //Debug.Log("Health " + health);
                gamerController.UpdateHealth(characterObjects[i],-health);
            }
        }
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(exp, 2);
    }
}
