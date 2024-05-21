using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float ChaseRadius = 30;
    public int HP = 100;
    public float MoveSpeed = 0.5f;
    private float Y = 0;
    public List<Transform> WalkPoints;
    public int CurrentPoint = 0;
    public bool ChangeDirection = false;
    public float ChangeDirectionTime;
    public float Damage = 20f;
    public float DamageDelay = 1.5f;
    public List<AudioClip> EnemySounds;
    void Start()
    {
        transform.rotation = Quaternion.LookRotation(WalkPoints[1].position - transform.position);
        Y = transform.position.y;
        StartCoroutine(SoundEffects());        
    }
    public void Chase()         
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead)
        {
           transform.rotation = Quaternion.LookRotation(GameObject.Find("doomguy2").transform.position - transform.position);
        }        
    }    
    void Update()
    {        
        transform.position = new Vector3(transform.position.x, Y, transform.position.z);        
        if(HP <= 0) 
        {
            GetComponent<AudioSource>().clip = EnemySounds[3];
            GetComponent<AudioSource>().Play();
            Destroy(gameObject); 
        }
        if(Vector3.Distance(transform.position, GameObject.Find("doomguy2").transform.position) < ChaseRadius && !GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead)
        {
            Chase();
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(WalkPoints[CurrentPoint].position - transform.position);
        }
        transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Move();
    }
    public void TakeDamage(int Amount)
    {
        HP -= Amount;
    }
    public void Move()
    {                    
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PathPoint")
        {
            int NextPoint = CurrentPoint + 1 >= WalkPoints.Count ? 0 : CurrentPoint + 1;            
            transform.rotation = Quaternion.LookRotation(WalkPoints[NextPoint].position - transform.position);
            CurrentPoint = CurrentPoint + 1 >= WalkPoints.Count ? 0 : CurrentPoint + 1;            
        }
    }
    public IEnumerator SoundEffects()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 20));
            GetComponent<AudioSource>().clip = EnemySounds[3];
            GetComponent<AudioSource>().Play();
        }        
    }
}