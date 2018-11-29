using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class manager : MonoBehaviour
{
    public GameObject title;
    public GameObject tag;
    public GameObject start;
    public GameObject intro;
    public GameObject story;
    public GameObject load;
    public AudioSource menuSound;
    private Animator titleAnim;
    private Animator tagAnim;
    private SceneState state;
    // Use this for initialization
    enum SceneState
    {
        INTRO,
        STORY,
        LOAD_SC,
        LOAD
    }; 

    void Start()
    {
        titleAnim = title.GetComponent<Animator>();
        tagAnim = tag.GetComponent<Animator>();
        titleAnim.SetBool("isHidden", false);
        tagAnim.SetBool("isHidden", false);
        state = SceneState.INTRO;
        start.GetComponent<Animator>().SetBool("active", false);
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
            if (state == SceneState.INTRO)
            {
                titleAnim.SetBool("isHidden", true);
                tagAnim.SetBool("isHidden", true);
                start.GetComponent<Animator>().SetBool("active", true);
                start.SetActive(false);
                intro.GetComponent<Animator>().SetBool("active", true);
                state = SceneState.STORY;

            } 
            else if (state == SceneState.STORY)
            {
                intro.GetComponent<Animator>().SetBool("active", false);
                story.GetComponent<Animator>().SetBool("active", true);
                state = SceneState.LOAD_SC;
            }
            else if(state == SceneState.LOAD_SC)
            {
                story.GetComponent<Animator>().SetBool("active", false);
                load.GetComponent<Animator>().SetBool("active", true);
                state = SceneState.LOAD;
            }
            else if (state == SceneState.LOAD)
            {
                load.GetComponent<Animator>().SetBool("active", false);

                //System.Threading.Thread.Sleep(3000);
                SceneManager.LoadScene(1);
            }

        }
    }
}
