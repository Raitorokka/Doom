using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public bool Closed = false;
    public bool IsOpen = false;
    private Vector3 start;
    private Vector3 end;
    // Start is called before the first frame update
    void Start()
    {        
        start = transform.position;
        end = transform.position + new Vector3(0f, 20.8f);        
    }
    public IEnumerator OpenDoor()
    {
        float time = 0f;
        if (!Closed)
        {
            while (time <= 1f)
            {
                time += 0.7f * Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, time);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (time <= 1f)
            {
                time += 0.7f * Time.deltaTime;
                transform.position = Vector3.Lerp(end, start, time);
                yield return new WaitForEndOfFrame();
            }
        }
        IsOpen = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (IsOpen)
        {
            if(transform.position == start)
            {
                Closed = false;
            }
            else if (transform.position == end)
            {
                Closed = true;
            }
            StartCoroutine(OpenDoor());
        }
    }
}
