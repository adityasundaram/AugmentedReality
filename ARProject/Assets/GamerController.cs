using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

using Global;

public class GamerController : MonoBehaviour {

    // ARCore object
    private DetectedPlane detectedPlane;
    public Camera firstPersonCamera;

    // Environment Game Object
    private GameObject environmentMap;

    public GameObject envPrefab;
    public GameObject char1Prefab;
    public GameObject char2Prefab;

    public GameObject char1health;
    public GameObject char2health;
    public GameObject charEnergy;


    // Contains all characters who have been instantiated
    private List<CharacterObject> characterObjects;

    // This will hold the current players context
    private CharacterObject currentPlayerObject;

    // Holds the index of the player in control
    private int currentPlayer;

    // Used while switching the player
    public Canvas popupCanvas;

    // Char energy value - for switching
    private float charEnergyValue;


    private Dictionary<string, GameObject> characterHealthIndex;

    private float speed = 0.4f;

    // Initializing functions

    public List<string> GetCharacterWeaponList(string prefabName)
    {
        Debug.Log(prefabName);
        List<string> weaponList = new List<string>();

        switch(prefabName){
            case "CyborgGirl(Clone)":
                weaponList = new List<string> { "Empty", "YM-3 Pistols", "Two Pistols", "YM-27 Rifle" };
                break;
            case "SciFiEngineer(Clone)":
                weaponList = new List<string> { "Empty", "One Pistol", "Two Pistols" };
                break;
            case "Sci-Fi_Soldier(Clone)":
                weaponList = new List<string> { "Empty", "Rifle"};
                break;
            case "Soldier(Clone)":
                weaponList = new List<string> { "Empty", "Sniper Rifle", "AK-74M" };
                break;
            case "SportyGirl(Clone)":
                weaponList = new List<string> { "Empty"};
                break;
            case "ContractKiller(Clone)":
                weaponList = new List<string> { "Empty", "One Pistol", "Two Pistols" };
                break;
            default:
                break;
        }

        return weaponList;
    }


    public CharacterObject createPlayerObject(GameObject character, GameObject health)
    {
        CharacterObject result = new CharacterObject();

        result.WeaponList = GetCharacterWeaponList(character.name);
        result.WeaponAllowed = true;
        result.currentWeaponIndex = 0;

        result.charObject = character;
        result.charController = character.GetComponent<PlayerController>();
        result.charActions = character.GetComponent<Actions>();
        result.health = health;

        return result;
    }

    void Start()
    {

        //Debug.Log(char1Prefab.transform.name);

        characterObjects = new List<CharacterObject>();

        charEnergyValue = 100f;
        characterHealthIndex = new Dictionary<string, GameObject>();
        characterHealthIndex.Add("Player_1",char1health);
        characterHealthIndex.Add("Player_2",char2health);

        popupCanvas.enabled = false;
    }



    // Update is called once per frame
    void Update()
    {

        if(CrossPlatformInputManager.GetButton("OK")){
            popupCanvas.enabled = false;
        }

        if(!popupCanvas.enabled)
        {

            // pass the input to the dude!
            if (characterObjects != null && characterObjects.Capacity > 1 && characterObjects.Capacity > 1)
            {
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");

                // Walking
                if (h != 0 || v != 0)
                {
                    UpdateEnergy(0.05f);
                    currentPlayerObject.charActions.Walk();
                    Vector3 target = new Vector3(h * speed * Time.deltaTime, 0, v * speed * Time.deltaTime);
                    target = firstPersonCamera.transform.TransformDirection(target);
                    target.y = 0;
                    Vector3 pos = currentPlayerObject.charController.transform.position + target;
                    currentPlayerObject.charController.transform.position = Vector3.Lerp(currentPlayerObject.charController.transform.position, pos, speed);
                    var rotation = Quaternion.LookRotation(target);
                    currentPlayerObject.charObject.transform.rotation = Quaternion.Slerp(
                                            currentPlayerObject.charObject.transform.rotation, 
                                            rotation,
                                            Time.deltaTime * 30f);
                }
                else if (CrossPlatformInputManager.GetButton("SwitchWeapon"))
                {
                    currentPlayerObject.charController.SetArsenal(GetWeaponName());
                    currentPlayerObject.charActions.Aiming();
                }
                else
                {
                    if (currentPlayerObject.currentWeaponIndex != 0)
                    {
                        currentPlayerObject.charActions.Aiming();
                    }
                    else
                    {
                        currentPlayerObject.charActions.Stay();
                    }
                }
            }
        }
    }

    public void SwitchPlayer(){
    
        currentPlayerObject.charActions.Stay();
        currentPlayer = (currentPlayer + 1) % 2;
        currentPlayerObject = characterObjects[currentPlayer];
        popupCanvas.enabled = true;
    }

    public string GetWeaponName()
    {
        if (currentPlayerObject.WeaponAllowed)
        {
            currentPlayerObject.currentWeaponIndex = (currentPlayerObject.currentWeaponIndex + 1) % currentPlayerObject.WeaponList.Capacity;
        }
        return currentPlayerObject.WeaponList[currentPlayerObject.currentWeaponIndex];
    }

    public void UpdateEnergy(float value){
        float energy = float.Parse(charEnergy.GetComponent<UnityEngine.UI.Text>().text);
        charEnergyValue -= value;

        if(charEnergyValue <= 0f){
            charEnergyValue = 100f;
            energy = 100f;
            SwitchPlayer();
        }else{
            if(energy - charEnergyValue>1){
                energy = Mathf.Ceil(charEnergyValue);
            }
        }
        charEnergy.GetComponent<UnityEngine.UI.Text>().text = (Mathf.Ceil(energy)).ToString();
    }


    public CharacterObject GetCharacterProperties(int index){
        return characterObjects[index];
    }

    public CharacterObject GetCurrentCharacterProperties(){
        return GetCharacterProperties(currentPlayer);
    }

    public int GetCurrentPlayerIndex(){
        return currentPlayer;
    }

    public void UpdateHealth(CharacterObject player, float health){

        float current_health = float.Parse(player.health.GetComponent<UnityEngine.UI.Text>().text);

        if (current_health > 0)
        {
            current_health += health;
            if (current_health < 0)
            {
                player.charActions.Death();
                current_health = 0;
            }else if(current_health > 100f){
                current_health = 100f;
            }

            player.health.GetComponent<UnityEngine.UI.Text>().text = (current_health).ToString();
        }
    }

    public void TakeDamage(int playerIndex){
        CharacterObject player = characterObjects[playerIndex];
        player.charActions.Damage();
        UpdateHealth(player,-2f);
    }

    // This will set the plane and spawn the character
    public void SetPlane(DetectedPlane plane, Anchor anchor, TrackableHit hit)
    {
        detectedPlane = plane;
        SpawnCharacter(anchor, hit);
    }

    // Initialzing the characters from the prefab
    void SpawnCharacter(Anchor anchor, TrackableHit hit)
    {
        if (characterObjects.Capacity < 2)
        {
            // Getting the pose of the detected plane
            //Vector3 pos = detectedPlane.CenterPose.position;
            //Vector3 spos = detectedPlane.CenterPose.position;
            Vector3 pos = hit.Pose.position;
            Vector3 spos = pos;
            // Setting the environemnt anchor
            Anchor envAnchor = anchor;


            // Instantiating the environemnt map
            environmentMap = Instantiate(envPrefab, pos, Quaternion.identity, anchor.transform);

            //Get All spawn locations
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("food");
            Vector3 loc1 = spawns[0].transform.position;
            Vector3 loc2 = spawns[1].transform.position;
            Destroy(spawns[0]);
            Destroy(spawns[1]);
            // Instantiating both the characters in different locations
            GameObject character1 = Instantiate(char1Prefab, loc1, Quaternion.identity, transform);
            //spos.x += 0.5f;
            //spos.z += 0.5f;
            GameObject character2 = Instantiate(char2Prefab, loc2, Quaternion.identity, transform);

            Vector3 newPos = new Vector3(firstPersonCamera.transform.position.x, spos.y, firstPersonCamera.transform.position.z);


            // Adding two players 
            // We have to create this because the weaponlist is initialized with the initial name
            characterObjects.Add(createPlayerObject(character1, char1health));
            characterObjects.Add(createPlayerObject(character2, char2health));


            // Changing the name of the objects and their looking pose
            characterObjects[0].charObject.transform.name = "Player_1";
            characterObjects[0].charObject.transform.LookAt(loc2);
            characterObjects[1].charObject.transform.name = "Player_2";
            characterObjects[1].charObject.transform.LookAt(loc1);

            // Fixing up the anchors for the game objects 
            newPos.y = detectedPlane.CenterPose.position.y;

            // Starting with the first player
            currentPlayer = 0;
            currentPlayerObject = characterObjects[0];
        }
    }
}
