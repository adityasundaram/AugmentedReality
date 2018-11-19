using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class manager : MonoBehaviour
{
    public GameObject title;
    public GameObject tag;
    private Animator titleAnim;
    private Animator tagAnim;
    private bool slideDown;
    // Use this for initialization
    void Start()
    {
        titleAnim = title.GetComponent<Animator>();
        tagAnim = tag.GetComponent<Animator>();
        titleAnim.SetBool("isHidden", false);
        tagAnim.SetBool("isHidden", false);
        slideDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTouches();
    }

    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount != 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
