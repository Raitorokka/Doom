using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDoor : MonoBehaviour
{
    public bool IsOpen = false;
    public Animator AR;
    public bool closed = false;

    // Start is called before the first frame update
    void Start()
    {
        AR = GetComponent<Animator>();  
        AR.SetInteger("IsClosing", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (closed)
        {            
            GetComponent<BoxCollider>().enabled = true;
            AR.SetInteger("IsClosing", 0);
        }
        else if (IsOpen)
        {
            closed = false;
            StartCoroutine(OpenDoor());
            GetComponent<BoxCollider>().enabled = false;            
        }
    }
    public IEnumerator OpenDoor()
    {
        IsOpen = false;
        AR.SetInteger("IsClosing", 2);
        yield return new WaitForSeconds(5f);
        AR.SetInteger("IsClosing", 1);        
        closed = true;
    }
}
