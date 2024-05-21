using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exitlevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {        
        if(other.tag == "Player")
        {
            StartCoroutine(ExitLevel());
        }
    }
    public IEnumerator ExitLevel()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("L2");
    }
}
