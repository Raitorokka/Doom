using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDemonHealth : MonoBehaviour
{
    public float HP = 150;
    public GameObject ParentObject;
    public Animator anim;
    public BlackDemon BlackDemonScript;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {        
        if (HP <= 0)
        {
            ParentObject.GetComponent<BlackDemon>().Death();            
        }
    }
    public void TakeDamage()
    {
        StartCoroutine(HitAnim());
        HP -= 20;
    }
    public IEnumerator HitAnim()
    {
        anim.SetBool("GetHit", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("GetHit", false);
        ParentObject.GetComponent<AudioSource>().clip = BlackDemonScript.BlackDemonSounds[3];
        ParentObject.GetComponent<AudioSource>().Play();
    }
}
