using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    public bool Show = false;
    public float fadeSpeed = 1.5f;
    
    void Start()
    {
        GetComponent<Text>().enabled = false;        
    }    
    // Update is called once per frame
    void Update()
    {
        if (Show)
        {
            GetComponent<Text>().enabled = true;
            GetComponent<Text>().color = new Color(
                GetComponent<Text>().color.r,
                GetComponent<Text>().color.g,
                GetComponent<Text>().color.b,
                Mathf.MoveTowards(GetComponent<Text>().color.a, 0f, fadeSpeed * Time.deltaTime));
            Show = GetComponent<Text>().color.a != 0f;
        }
        else
        {
            GetComponent<Text>().color = new Color(
                GetComponent<Text>().color.r,
                GetComponent<Text>().color.g,
                GetComponent<Text>().color.b, 1f);
            GetComponent<Text>().enabled = false;
        }

    }
}
