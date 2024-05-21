using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(GameObject.Find("doomguy2").transform.position.x, transform.position.y, GameObject.Find("doomguy2").transform.position.z);
    }
}
