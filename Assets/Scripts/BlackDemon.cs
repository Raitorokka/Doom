using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackDemon : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Animator Anim;
    public Transform[] PatrolPoints;
    public int PatrolPoint;
    public enum State
    {
        Idle, Patrol, Chase, Attack
    };
    public State state;
    public float WaitAtPoint = 3f;
    public float WaitAtPointTimer;
    public float ChaseRange = 5f;
    public float AttackRange = 1f;
    public float AttackDelay = 2f;    
    public float AttackDelayTimer;
    public float rotationSpeed = 3f;
    public GameObject CutScene;   
    public GameObject StartCamera;
    public GameObject EndCamera;
    public GameObject VictoryText;
    public List<AudioClip> BlackDemonSounds;
    public BlackDemonHealth health;
    public bool IsIdle = true;
    // Start is called before the first frame update
    void Start()
    {
    
    }


    // Update is called once per frame
    void Update()
    {
        if (health.HP <= 0)
        {
            GetComponent<AudioSource>().clip = BlackDemonSounds[1];            
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        else if (Random.Range(0, 1000) == 5 && IsIdle)
        {
           // GetComponent<AudioSource>().clip = BlackDemonSounds[2];
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().PlayOneShot(BlackDemonSounds[2], 1f);
        }
        float DistanceToPlayer = Vector3.Distance(GameObject.Find("doomguy2").transform.position, Agent.transform.position);

        switch (state)
        {
            case State.Idle:
                Idle(DistanceToPlayer);
                break;
            case State.Patrol:
                Patrol(DistanceToPlayer);
                break;
            case State.Chase:
                Chase(DistanceToPlayer);
                break;
            case State.Attack:
                Attack(DistanceToPlayer);
                break;
        }
    }
    private void Idle(float DistanceToPlayer)
    {
        IsIdle = true;
        if (DistanceToPlayer <= ChaseRange) { state = State.Chase; }
        else
        {
            Anim.SetBool("IsWalking", false);
            if(WaitAtPointTimer > 0) { WaitAtPointTimer -= Time.deltaTime; }
            else
            {
                state = State.Patrol;
                Agent.SetDestination(PatrolPoints[PatrolPoint].position);
            }
        }
    }
    private void Patrol(float DistanceToPlayer)
    {
        IsIdle = true;
        if (DistanceToPlayer <= ChaseRange) { state = State.Chase; }
        else
        {
            LookAtSlerp(PatrolPoints[PatrolPoint]);
            bool IsWalking = true;
            if(Agent.remainingDistance <= 0.2f)
            {
                PatrolPoint++;
                if (PatrolPoint == PatrolPoints.Length) PatrolPoint = 0;
                state = State.Idle;
                WaitAtPointTimer += WaitAtPoint;
                IsWalking = false;
            }
            Anim.SetBool("IsWalking", IsWalking);
        }
    }
    private void Chase(float distanceToPlayer)
    {
        
        LookAtSlerp(GameObject.Find("doomguy2").transform);
        Agent.SetDestination(GameObject.Find("doomguy2").transform.position);
        bool IsWalking = true;
        if(distanceToPlayer <= AttackRange)
        {
            IsIdle = false;
            state = State.Attack;
            GetComponent<AudioSource>().clip = BlackDemonSounds[0];
            GetComponent<AudioSource>().Play();
            IsWalking = false;
            Anim.SetBool("Punched", true);
            Agent.velocity = Vector3.zero;
            Agent.isStopped = true;

            AttackDelayTimer = AttackDelay;
        }
        else if(distanceToPlayer > ChaseRange)
        {
            IsIdle = true;
            state = State.Patrol;
            WaitAtPointTimer = WaitAtPoint;
            Agent.velocity = Vector3.zero;
            Agent.SetDestination(Agent.transform.position);
        }
        Anim.SetBool("IsWalking", IsWalking);
    }
    private void Attack(float distanceToPlayer)
    {
        LookAtSlerp(GameObject.Find("doomguy2").transform);
        AttackDelayTimer -= Time.deltaTime;
        if (AttackDelayTimer <= 0)
        {                
            if (distanceToPlayer <= AttackRange)
            {   
                IsIdle = false;
                GetComponent<AudioSource>().clip = BlackDemonSounds[0];
                GetComponent<AudioSource>().Play();
                Anim.SetBool("Punched", true);
                AttackDelayTimer = AttackDelay;
            }
            else
            {
                IsIdle = true;
                Anim.SetBool("Punched", false);
                state = State.Idle;
                Agent.isStopped = false;              
            }
            
        }
        else if (AttackDelayTimer > 0 && AttackDelayTimer < 2) { Anim.SetBool("Punched", false); } 
    }
    public void LookAtSlerp(Transform target)
    {
        Agent.transform.rotation = Quaternion.Slerp(Agent.transform.rotation, Quaternion.LookRotation(target.transform.position
            - Agent.transform.position), Time.deltaTime * rotationSpeed);
        Agent.transform.rotation = Quaternion.Euler(0f, Agent.transform.rotation.eulerAngles.y, 0f);
    }
    public void Death()
    {        
        CutScene.SetActive(true);
        StartCamera.SetActive(true);
        EndCamera.SetActive(true);        
        GameObject.Find("doomguy2").GetComponent<Player>().enabled = false;
        GameObject.Find("doomguy2").GetComponent<CameraRotate>().enabled = false;
        Anim.SetBool("IsDead", true);                
        GameObject.Find("doomguy2/Main Camera/WeaponLocation").SetActive(false);
        GameObject.Find("doomguy2/Main Camera/doomslayer_arm_left").SetActive(false);
        GameObject.Find("doomguy2/Main Camera/doomslayer_arm_right").SetActive(false);        
        GameObject.Find("HUD/UI").SetActive(false);
        enabled = false;
        GameObject.Find("doomguy2").GetComponent<Player>().AnimationsOff();
        GameObject.Find("doomguy2/AnimationModel/doomslayer_arm_left").SetActive(true);
        GameObject.Find("doomguy2/AnimationModel/doomslayer_arm_right").SetActive(true);
    }
    public void Victorytext()
    {
        Cursor.lockState = CursorLockMode.None;
        VictoryText.SetActive(true);
        GameObject.Find("doomguy2").GetComponent<PlayerHealth>().BackToMenu.SetActive(true);
    }    
}
