using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingmanager : MonoBehaviour
{

    public GameObject flashingObject;
    bool toBlink = true;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Blink());
        //StartCoroutine(StopBlink());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //CoRoutine to perform blinking
    IEnumerator Blink()
    {
        while (toBlink)
        {
            //Making the text visible for 0.5 seconds
            flashingObject.transform.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            //Making the text invisible for 0.5 seconds
            flashingObject.transform.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
    //CoRoutine to stop blinking after 5 seconds
    IEnumerator StopBlink()
    {
        yield return new WaitForSeconds(5f);
        toBlink = false;
    }
}