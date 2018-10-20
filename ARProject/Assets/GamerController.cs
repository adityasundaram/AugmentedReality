using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

public class GamerController : MonoBehaviour {

    private DetectedPlane detectedPlane;
    public Camera firstPersonCamera;


    private List<PlayerController> controllers;

    // Controls which player we are controlling
    private int currentPlayer;
    private GameObject currentCharacter;
    private Actions currentActions;
    private PlayerController currentPlayerController;

    // References of the player in the game 
    private List<GameObject> characters;

    // Gameobject created
    private GameObject desertMap;

    public GameObject envPrefab;
    public GameObject char1Prefab;
    public GameObject char2Prefab;

    public GameObject char1health;
    public GameObject char2health;

    private List<string> WeaponList = new List<string> {"Empty","One Pistol","Two Pistols"};
    private Dictionary<string,List<int>> allowedWeaponIndex = new Dictionary<string, List<int>> { { "SciFiEngineer_1",new List<int>{0}} , { "SciFiEngineer_2", new List<int> { 0 } } };
    private List<int> Player2_allow = new List<int> {0};
    private Dictionary<string,int> characterWeaponIndex = new Dictionary<string, int> {{ "SciFiEngineer_1",0}, {"SciFiEngineer_2",0}};
    private Dictionary<string, GameObject> characterHealthIndex;

    private float speed = 0.4f;

    void Start()
    {
        characters = new List<GameObject>();
        controllers = new List<PlayerController>();

        characterHealthIndex = new Dictionary<string, GameObject>();
        characterHealthIndex.Add("SciFiEngineer_1",char1health);
        characterHealthIndex.Add("SciFiEngineer_2",char2health);
    }

    // Update is called once per frame
    void Update()
    {
        // pass the input to the dude!
        if (controllers != null && controllers.Capacity > 1 && characters.Capacity > 1)
        {
            currentCharacter = characters[currentPlayer];
            currentActions = currentCharacter.GetComponent<Actions>();
            currentPlayerController = currentCharacter.GetComponent<PlayerController>();

            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (h !=0 || v != 0)
            {
                currentActions.Walk();
                Vector3 target = new Vector3(h * speed * Time.deltaTime , 0 , v * speed * Time.deltaTime);
                Vector3 pos = currentCharacter.transform.position + target;
                currentCharacter.transform.position = Vector3.Lerp(currentCharacter.transform.position , pos, speed);
                var rotation = Quaternion.LookRotation(target);
                currentCharacter.transform.rotation = Quaternion.Slerp(currentCharacter.transform.rotation, rotation, Time.deltaTime * 30f);
            }
            else if (CrossPlatformInputManager.GetButton("SwitchWeapon"))
            {
                //Debug.Log("Switch Weapon was pressed");
                currentPlayerController.SetArsenal(WeaponList[GetWeaponIndex()]);
                currentActions.Aiming();
            }
            else
            {
                currentActions.Stay();
            }

            // Get all thhe components of the switched charater and populate the Global Variables
            if(CrossPlatformInputManager.GetButton("Switch"))
            {
                currentPlayer = (currentPlayer + 1) % 2;
                currentCharacter = characters[currentPlayer];
                currentActions = currentCharacter.GetComponent<Actions>();
                currentPlayerController = currentCharacter.GetComponent<PlayerController>();
            }
        }
    }

    public int GetWeaponIndex(){

        characterWeaponIndex[currentCharacter.name] += 1;
        while(!allowedWeaponIndex[currentCharacter.name].Contains(characterWeaponIndex[currentCharacter.name] % WeaponList.Count)){
            characterWeaponIndex[currentCharacter.name] += 1;
        }
        return characterWeaponIndex[currentCharacter.name] % WeaponList.Count;
    }

    public Dictionary<string,GameObject> getCharacterHealthDict(){
        return characterHealthIndex;
    }

    public GameObject getCurrentCharacter(){
        return currentCharacter;
    }

    public Actions getCurrentActions(){
        return currentActions;
    }

    public Dictionary<string,List<int>> getAllowedWeaponIndex(){
        return allowedWeaponIndex;
    }

    public void SetPlane(DetectedPlane plane, Anchor anchor)
    {
        detectedPlane = plane;
        SpawnCharacter(anchor);
    }

    void SpawnCharacter(Anchor anchor)
    {
        if (characters.Capacity < 2)
        {
            Vector3 pos = detectedPlane.CenterPose.position;
            Vector3 spos = detectedPlane.CenterPose.position;
            spos.y += 2.0f;
            Anchor envAnchor = anchor;
            desertMap = Instantiate(envPrefab, pos, Quaternion.identity, transform);
            GameObject character = Instantiate(char1Prefab, spos, Quaternion.identity, transform);
            spos.x += 1.0f;
            spos.z += 0.5f;
            GameObject character2 = Instantiate(char2Prefab, spos, Quaternion.identity, transform);
            //desertMap.transform.Find("SciFiEngineer").gameObject;
            Vector3 newPos = new Vector3(firstPersonCamera.transform.position.x, spos.y, firstPersonCamera.transform.position.z);
            controllers.Add(character.GetComponent<PlayerController>());
            controllers.Add(character2.GetComponent<PlayerController>());
            characters.Add(character);
            characters.Add(character2);
            currentPlayer = 0;
            character.transform.name = "SciFiEngineer_1";
            character.transform.LookAt(newPos);
            character2.transform.name = "SciFiEngineer_2";
            character2.transform.LookAt(newPos);
            newPos.y = detectedPlane.CenterPose.position.y;
            desertMap.transform.LookAt(newPos);
            desertMap.transform.parent = envAnchor.transform;
            character.transform.parent = envAnchor.transform;
            character2.transform.parent = envAnchor.transform;


        }
       
    }
}
