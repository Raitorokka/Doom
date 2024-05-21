using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{    
    public int DamageDeallingEnemies;
    public Collider DamageDealler;
    public float Armor = 100;
    public float Health = 100f;
    public float DamageDelay = 1f;
    public float CurrentDamageDelay = 0;
    public Image HitScreen;
    public bool FadeToRed = true;
    public bool FadeFromRed = false;
    public bool TakingDamage = false;
    public Animator animator;
    public Slider ArmorSlider;
    public Slider HPSlider;
    public float dif = 0f;
    public float falling_timer;
    public float startY;
    public float endY;
    public GameObject GameOver;
    public GameObject BackToMenu;
    public GameObject Respawn;
    public bool PlayerDead = false;
    public Player Player;        

    // Start is called before the first frame update
    void Start()
    {
        Respawn = GameObject.Find("HUD/UI/Respawn");
        Respawn.SetActive(false);
        BackToMenu = GameObject.Find("HUD/BackToMenu");
        BackToMenu.SetActive(false);
        GameOver = GameObject.Find("HUD/UI/GameOver");
        GameOver.SetActive(false);
        HitScreen = GameObject.Find("HUD/UI/HitScreen").GetComponent<Image>();
        StartCoroutine(FirstEnemyDealDamage());
        ArmorSlider = GameObject.Find("HUD/UI/ArmorSlider").GetComponent<Slider>();
        HPSlider = GameObject.Find("HUD/UI/HpSlider").GetComponent<Slider>();
        Player = GameObject.Find("doomguy2").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
        if (CurrentDamageDelay >= 0)
        {
            CurrentDamageDelay -= Time.deltaTime;
        }
        HPSlider.value = Health / 100;
        ArmorSlider.value = Armor / 100;                
        FallingDamage();
        if (PlayerDead)
        {
            Player.enabled = false;
            
        }
        Death();
    }
    public void FallingDamage()
    {
        if (GetComponent<Player>().grounded)
        {
            startY = transform.position.y;
        }
        float damage = 0;
        while (dif >= 20)
        {
            dif -= 20;
            damage += 20;
        }
        damage += dif * 0.7f;
        if (Armor < 0)
        {
            Health -= damage;
        }
        else
        {
            Armor -= damage;
            if (Armor <= 0)
            {
                float dd = Mathf.Abs(Armor);
                Health -= (damage - dd) / 2;
                Health -= dd;
            }
            else
            {
                Health -= damage / 2;
            }
        }

        dif = 0;
        if (!GetComponent<Player>().grounded)
        {
            falling_timer += Time.deltaTime;
            endY = transform.position.y;

            if (dif < startY - endY)
            {
                dif = startY - endY;
            }
        }
    }
    public void Death()
    {
        if (Health <= 0)
        {            
            GameOver.SetActive(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead = true;
            animator.SetBool("IsJumped", false);
            animator.SetBool("IsSprinting", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDead", true);            
            BackToMenu.SetActive(true);
            Respawn.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;            
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Aid")
        {
            Destroy(other.gameObject);
            if (Health + 40 <= 100)
            {
                Destroy(other.gameObject);
                Health += 40;
            }
            else
            {
                Destroy(other.gameObject);
                Health = 100;
            }
        }
        if (other.tag == "Armor")
        {
            Destroy(other.gameObject);
            if (Armor + 40 <= 100)
            {
                Destroy(other.gameObject);
                Armor += 40;
            }
            else
            {
                Destroy(other.gameObject);
                Armor = 100;
            }
        }
        if (other.tag == "Enemy")
        {
            DamageDeallingEnemies++;
            TakingDamage = true;
            DamageDealler = other;
            GetComponent<AudioSource>().clip = other.gameObject.GetComponent<Enemy>().EnemySounds[1];
            GetComponent<AudioSource>().volume = 1;
            GetComponent<AudioSource>().Play();
            StartCoroutine(ReciveDamage());
            FadeToRed = true;
        }
        if (other.tag == "BlackDemon" && CurrentDamageDelay <= 0)
        {
            CurrentDamageDelay = DamageDelay;
            BlackDemonDamage(other);
            StartCoroutine(ReciveDamage());
            FadeToRed = true;
        }
    }
    public IEnumerator ReciveDamage()
    {
        while (FadeToRed)
        {
            HitScreen.color = new Color(
                HitScreen.color.r,
                HitScreen.color.g,
                HitScreen.color.b,
                Mathf.MoveTowards(HitScreen.color.a, 0.5f, Time.deltaTime)
            );
            yield return new WaitForSeconds(0.0000000000000000000000000000000000000000000000000000001f);
            FadeToRed = HitScreen.color.a != 0.5f;
            FadeFromRed = true;
        }
        while (FadeFromRed)
        {
            HitScreen.color = new Color(
                HitScreen.color.r,
                HitScreen.color.g,
                HitScreen.color.b,
                Mathf.MoveTowards(HitScreen.color.a, 0f, Time.deltaTime)
            );
            yield return new WaitForSeconds(0.1f);
            FadeFromRed = HitScreen.color.a != 0;
        }
    }
    public IEnumerator FirstEnemyDealDamage()
    {
        while (true)
        {
            if (TakingDamage)
            {
                GetComponent<AudioSource>().clip = DamageDealler.gameObject.GetComponent<Enemy>().EnemySounds[3];
                GetComponent<AudioSource>().volume = 1;
                GetComponent<AudioSource>().Play();
                float EnemyDamage = DamageDealler.gameObject.GetComponent<Enemy>().Damage * DamageDeallingEnemies;
                DealDamage(EnemyDamage);
                yield return new WaitForSeconds(DamageDealler.gameObject.GetComponent<Enemy>().DamageDelay);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DealDamage(float EnemyDamage)
    {
        if (Armor < 0)
        {
            Health -= EnemyDamage;
        }
        else
        {
            Armor -= EnemyDamage;
            if (Armor <= 0)
            {
                float dd = Mathf.Abs(Armor);
                Health -= (EnemyDamage - dd) / 2;
                Health -= dd;
            }
            else
            {
                Health -= EnemyDamage / 2;
            }
        }
    }
    public void BlackDemonDamage(Collider EnemyCollider)
    {
        float EnemyDamage = EnemyCollider.gameObject.GetComponent<BlackDemonDamage>().Damage;
        DealDamage(EnemyDamage);
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            DamageDeallingEnemies--;
            DamageDealler = null;
            TakingDamage = false;
        }
    }
}
