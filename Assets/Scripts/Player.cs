using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{

    public CharacterController character;
    public Animator animator;
    public float moveSpeed = 5f;
    public float runSpeed = 30f;
    public float jumpHeight = 20f;
    public float gravityScale = 5;
    public bool grounded;
    private Vector3 motion;




    public int ShotgunAmmo = 12;
    public int PlasmarifleAmmo = 60;
    public CameraRotate cam;
    public bool IsWeapon = false;
    public int WeaponId = -1;
    public GameObject[] Weapons;
    public Transform WeaponLocation;
    public Vector3 WeaponLocationPos;
    public Vector3 WeaponLocationRot;
    public GameObject Arm1;
    public GameObject Arm2;
    public GameObject WeaponArm1;
    public GameObject WeaponArm2;

    public Vector3 Arm1Pos;
    public Vector3 Arm2Pos;
    public Vector3 Arm1Rot;
    public Vector3 Arm2Rot;
    public float CurrentDelay = 0;
    public float ShootingTime = 0f;
    public float PlasmarifleRecoilX = 1;
    public float PlasmarifleRecoilY = 2;
    public float ShotgunRecoilX = 0.5f;
    public float ShotgunRecoilY = 50f;
    [HideInInspector]
    public Quaternion CameraRot;
    public Quaternion PlayerRot;
    public int PlasmarifleSpentAmmo;
    public int ShotgunSpentAmmo;
    public bool CanShootPlasmarifle = true;
    public bool CanShootShotgun = true;
    public Text ReloadText;
    public Text AmmoText;
    public bool CanReloadPlasmarifle = true;
    public bool CanReloadShotgun = true;
    public GameObject[] Head;
    public bool IsHeadHide = false;
    public GameObject PlasmaRifleSlot;
    public GameObject ShotgunSlot;
    public int FirstWeaponSlot = -1;
    public int SecondWeaponSlot = -1;
    public GameObject MainRootBone;
    public GameObject FirstWSlot;
    public GameObject SecondWSlot;
    public List<AudioClip> ShootSounds;

    public int ShootCount = 0;
    public float InteractRange = 100f;
    public Transform ParticlePoint;
    public Vector3 ParticlePointPosition;
    public Vector3 ParticlePointRotation;
    public GameObject PlasmarifleParticle;
    public GameObject ShotgunParticle;

    public bool HasKey = false;
    public GameObject Key;
    public GameObject PinKeyPad;
    public GameObject ActText;
    public bool IsKeyPad = false;
    public bool CanType = true;
    public float RecoilCount = 0f;
    public GameObject VictoryPlasmaRifle;
    public GameObject VictoryShotgun;
    public GameObject Pause;
    public GameObject TestCube;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "L2")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Key = GameObject.Find("Key");
            PinKeyPad = GameObject.Find("KeyPadHud/KeyPad");
            PinKeyPad.SetActive(false);
            ActText = GameObject.Find("KeyPadHud/Action Text");
            ActText.SetActive(false);
        }
        ParticlePoint = GameObject.Find("doomguy2/Main Camera/WeaponLocation/ParticlePoint").transform;
        MainRootBone = GameObject.Find("doomguy2/AnimationModel");
        grounded = character.isGrounded;
        cam = GameObject.Find("doomguy2").GetComponent<CameraRotate>();
        Arm1 = GameObject.Find("doomguy2/AnimationModel/doomslayer_arm_left");
        Arm2 = GameObject.Find("doomguy2/AnimationModel/doomslayer_arm_right");
        WeaponArm1 = GameObject.Find("doomguy2/Main Camera/doomslayer_arm_left");
        WeaponArm2 = GameObject.Find("doomguy2/Main Camera/doomslayer_arm_right");
        WeaponArm1.SetActive(false);
        WeaponArm2.SetActive(false);
        ReloadText = GameObject.Find("HUD/UI/ReloadText").GetComponent<Text>();
        ReloadText.enabled = false;
        AmmoText = GameObject.Find("HUD/UI/Ammo").GetComponent<Text>();
        PlasmaRifleSlot = GameObject.Find("HUD/UI/PlasmarifleSlot");
        ShotgunSlot = GameObject.Find("HUD/UI/ShotgunSlot");
        PlasmaRifleSlot.SetActive(false);
        ShotgunSlot.SetActive(false);
        VictoryPlasmaRifle.SetActive(false);
        VictoryShotgun.SetActive(false);
        Pause = GameObject.Find("HUD/Pause");
        Pause.SetActive(false);
        if (!Settings.doomguy2)
        {
            DontDestroyOnLoad(gameObject);
            Settings.doomguy2 = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().name == "AItestingscene")
        {
            CloseKeyPad();
            KeyPad();
        }
        CheckLook();
        if (Input.GetKeyDown(KeyCode.Alpha1) && FirstWeaponSlot != -1)
        {
            ChangeSlot1();
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha2) && SecondWeaponSlot != -1))
        {
            ChangeSlot2();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && IsWeapon)
        {
            ChangeSlot3();
        }
        if (WeaponId == 0)
        {
            AmmoText.text = PlasmarifleSpentAmmo.ToString() + " | " + PlasmarifleAmmo.ToString();
        }
        else if (WeaponId == 1)
        {
            AmmoText.text = ShotgunSpentAmmo.ToString() + " | " + ShotgunAmmo.ToString();
        }
        if (!CanShootPlasmarifle && IsWeapon && WeaponId == 0)
        {

            ReloadText.enabled = true;
            if ((Input.GetKey(KeyCode.R) || Input.GetMouseButtonDown(0)) && CanReloadPlasmarifle && PlasmarifleAmmo > 0 && PlasmarifleSpentAmmo < 30)
            {
                StartCoroutine(ReloadPlasmarifle());
            }
        }
        else if (Input.GetKey(KeyCode.R) && IsWeapon && CanReloadPlasmarifle && WeaponId == 0 && PlasmarifleSpentAmmo < 30)
        {
            StartCoroutine(ReloadPlasmarifle());
        }
        else if (!CanShootShotgun && IsWeapon && WeaponId == 1)
        {

            ReloadText.enabled = true;
            if ((Input.GetKey(KeyCode.R) || Input.GetMouseButtonDown(0)) && CanReloadShotgun && ShotgunAmmo > 0)
            {
                StartCoroutine(ReloadShotgun());
            }
        }
        else if (Input.GetKey(KeyCode.R) && IsWeapon && CanReloadShotgun && WeaponId == 1)
        {
            StartCoroutine(ReloadShotgun());
        }

        if (grounded)
        {
            motion.y = 0;
            CalculateMotion();
        }
        motion.y += gravityScale * Physics.gravity.y * Time.deltaTime;
        if (!IsKeyPad)
        {
            character.Move(motion * Time.deltaTime);
        }
        grounded = character.isGrounded;
        if (grounded)
        {
            animator.SetBool("IsJumped", false);
        }

        if (WeaponId == 1 && CurrentDelay <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                shoot();
                CurrentDelay = Weapon.ShotgunDelay;
            }
        }
        else if (WeaponId == 0 && CurrentDelay <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                shoot();
                ShootingTime += 1f;
                CurrentDelay = Weapon.PlasmarifleDelay;
            }
            else
            {
                ShootingTime = 0;
                RecoilCount = 0f;
            }
        }
        CurrentDelay -= Time.deltaTime;
        animator.SetBool("IsWeapon", IsWeapon);
        if (Input.GetKeyDown(KeyCode.Escape) && !Pause.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            Pause.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Pause.activeInHierarchy)
        {
            ExitPause();
        }
    }
    public void ExitPause()
    {
        Pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ChangeSlot1()
    {
        IsWeapon = true;
        FirstWSlot.SetActive(true);
        if (SecondWeaponSlot != -1)
        {
            SecondWSlot.SetActive(false);
        }

        if (FirstWeaponSlot == 0)
        {
            ShotgunSlot.SetActive(false);
            PlasmaRifleSlot.SetActive(true);
            WeaponId = 0;
            SetPositionPlasmarifle();
            SetZeroPosition();
        }
        else if (FirstWeaponSlot == 1)
        {
            ShotgunSlot.SetActive(true);
            PlasmaRifleSlot.SetActive(false);
            WeaponId = 1;
            SetPositionShotgun();
            SetZeroPosition();
        }
        WeaponArm1.SetActive(true);
        WeaponArm2.SetActive(true);
        Arm1.SetActive(false);
        Arm2.SetActive(false);
    }
    public void ChangeSlot2()
    {
        IsWeapon = true;
        FirstWSlot.SetActive(false);
        SecondWSlot.SetActive(true);
        if (SecondWeaponSlot == 0)
        {
            WeaponId = 0;
            ShotgunSlot.SetActive(false);
            PlasmaRifleSlot.SetActive(true);
            SetPositionPlasmarifle();
            SetZeroPosition();
        }
        else if (SecondWeaponSlot == 1)
        {
            WeaponId = 1;
            ShotgunSlot.SetActive(true);
            PlasmaRifleSlot.SetActive(false);
            SetPositionShotgun();
            SetZeroPosition();
        }
        WeaponArm1.SetActive(true);
        WeaponArm2.SetActive(true);
        Arm1.SetActive(false);
        Arm2.SetActive(false);
    }
    public void ChangeSlot3()
    {
        IsWeapon = false;
        if (FirstWeaponSlot != -1)
        {
            FirstWSlot.SetActive(false);
        }
        if (SecondWeaponSlot != -1)
        {
            SecondWSlot.SetActive(false);
        }
        ShotgunSlot.SetActive(false);
        PlasmaRifleSlot.SetActive(false);
        WeaponArm1.SetActive(false);
        WeaponArm2.SetActive(false);
        Arm1.SetActive(true);
        Arm2.SetActive(true);
    }
    public void EnterNumber(int N)
    {
        if (CanType) { PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text += N.ToString(); }
    }
    public void KeyPad()
    {
        Vector3 target = GameObject.Find("doomguy2/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Weapon.PlasmarifleShootRange));
        Vector3 direction = target - cam.Cam.position;
        Ray ray = new Ray(cam.Cam.position, direction.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Weapon.PlasmarifleShootRange))
        {
            if (hit.collider.gameObject.tag == "KeyPad" && !IsKeyPad)
            {
                ActText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LaunchKeyPad();
                }
            }
            else
            {
                ActText.SetActive(false);
            }
        }
    }
    public void LaunchKeyPad()
    {
        IsKeyPad = true;
        PinKeyPad.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ConfirmCode()
    {
        if (PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text == "1111")
        {
            CanType = false;
            PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Accepted";
            StartCoroutine(KeyPadDelay());
        }
        else
        {
            PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error";
            CanType = false;
            StartCoroutine(ErrorWait());
        }
    }
    public IEnumerator ErrorWait()
    {
        yield return new WaitForSeconds(1f);
        PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
        CanType = true;
    }
    public IEnumerator KeyPadDelay()
    {
        yield return new WaitForSeconds(1f);
        IsKeyPad = false;
        PinKeyPad.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Key Card Reader").GetComponent<BoxCollider>().enabled = false;
    }
    public void Erase()
    {
        if (CanType) { PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text = PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text.Substring(0, PinKeyPad.transform.GetChild(1).gameObject.GetComponent<Text>().text.Length - 1); }
    }
    public void CloseKeyPad()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsKeyPad)
        {
            IsKeyPad = false;
            PinKeyPad.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (IsKeyPad)
        {
            ActText.SetActive(false);
        }
    }

    private void CalculateMotion()
    {
        float CurrentHeight = motion.y;
        float verticalInput = Input.GetKey(Info.WBackward) ? -moveSpeed : Input.GetKey(Info.WForward) ? moveSpeed : 0;
        float horizontalInput = Input.GetKey(Info.WLeft) ? -moveSpeed : Input.GetKey(Info.WRight) ? moveSpeed : 0;
        motion = transform.forward * verticalInput + transform.right * horizontalInput;
        motion.Normalize();
        motion *= Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        motion.y = CurrentHeight;
        if (grounded && Input.GetButton("Jump"))
        {
            motion.y = jumpHeight;
            animator.SetBool("IsJumped", true);
        }
        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("IsWalking", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("IsSprinting", true);
                cam.IsRunning = true;
                if (!IsHeadHide)
                {
                    MainRootBone.transform.position = new Vector3(MainRootBone.transform.position.x, MainRootBone.transform.position.y, MainRootBone.transform.position.z + 0.01f);
                    IsHeadHide = true;
                }
            }
            else
            {
                animator.SetBool("IsSprinting", false);
                cam.IsRunning = false;
                if (IsHeadHide)
                {
                    MainRootBone.transform.position = new Vector3(MainRootBone.transform.position.x, MainRootBone.transform.position.y, MainRootBone.transform.position.z - 0.01f);
                    IsHeadHide = false;
                }

            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsSprinting", false);
            cam.IsRunning = false;
            if (IsHeadHide)
            {
                MainRootBone.transform.position = new Vector3(MainRootBone.transform.position.x, MainRootBone.transform.position.y, MainRootBone.transform.position.z - 0.01f);
                IsHeadHide = false;
            }
        }
    }
    public void shoot()
    {
        if (IsWeapon)
        {


            if (PlasmarifleSpentAmmo > 0 && CanShootPlasmarifle && WeaponId == 0)
            {
                PlasmaRifleShoot();
            }
            else if (ShotgunSpentAmmo > 0 && CanShootShotgun && WeaponId == 1)
            {
                ShotgunShoot();
            }
            if (ShotgunSpentAmmo <= 0)
            {
                CanShootShotgun = false;
            }

        }
    }
    public void PlasmaRifleShoot()
    {
        if (ShootCount == 5)
        {
            GetComponent<AudioSource>().clip = ShootSounds[1];
            GetComponent<AudioSource>().volume = 0.7f;
            GetComponent<AudioSource>().Play();
            ShootCount = 0;
        }
        if (Mathf.Abs(RecoilCount) < 40)
        {
            float rx = Random.Range(-2.5f, 0); ;
            float ry = Random.Range(-0.3f, 0.3f);
            GetComponent<CameraRotate>().camRotation.x += rx;
            GetComponent<CameraRotate>().camRotation.y += ry;
            RecoilCount += rx;
        }
        ShootCount++;
        GameObject P = Instantiate(PlasmarifleParticle, ParticlePoint.position, ParticlePoint.rotation);
        P.transform.SetParent(ParticlePoint);
        Destroy(P, 0.5f);
        PlasmarifleSpentAmmo--;
        if (PlasmarifleSpentAmmo <= 0)
        {
            CanShootPlasmarifle = false;
        }
        Vector3 target = GameObject.Find("doomguy2/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Weapon.PlasmarifleShootRange));
        Vector3 direction = target - cam.Cam.position;
        Ray ray = new Ray(cam.Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Weapon.PlasmarifleShootRange))
        {
            Instantiate(TestCube, hit.point, Quaternion.identity);
            if (hit.collider.gameObject.tag == "Enemy") hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(Weapon.PlasmarifleDamage);
            else if (hit.collider.gameObject.tag == "BlackDemonHitBox")
            {
                hit.collider.gameObject.GetComponent<BlackDemonHealth>().TakeDamage();
            }
        }
    }
    public void ShotgunShoot()
    {
        float PreviousX = GetComponent<CameraRotate>().Cam.localEulerAngles.x;
        GetComponent<AudioSource>().clip = ShootSounds[0];
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().Play();
        GetComponent<CameraRotate>().camRotation.x += Random.Range(-13f, -7);
        GetComponent<CameraRotate>().camRotation.y += Random.Range(-0.3f, 0.3f);
        ShotgunSpentAmmo--;
        Debug.Log(GetComponent<CameraRotate>().camRotation.x);
        for (int i = 0; i < Weapon.ShotgunShootCount; i++)
        {
            float SpreadX = Random.Range(-Weapon.ShotgunXSpread, Weapon.ShotgunXSpread);
            float SpreadY = Random.Range(-Weapon.ShotgunYSpread, Weapon.ShotgunYSpread);
            Vector3 target = GameObject.Find("doomguy2/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3((Screen.width / 2) + SpreadX, (Screen.height / 2 + SpreadY), Weapon.PlasmarifleShootRange));
            Vector3 direction = target - cam.Cam.position;
            Ray ray = new Ray(cam.Cam.position, direction.normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Weapon.ShotgunShootRange))
            {
                Instantiate(TestCube, hit.point, Quaternion.identity);
                if (hit.collider.gameObject.tag == "Enemy") hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(Weapon.PlasmarifleDamage);
                else if (hit.collider.gameObject.tag == "BlackDemonHitBox")
                {
                    hit.collider.gameObject.GetComponent<BlackDemonHealth>().TakeDamage();
                }
            }
        }
        GetComponent<CameraRotate>().IsReturning = true;
        GetComponent<CameraRotate>().PreviousX = PreviousX;
    }
    public void CheckLook()
    {
        Vector3 target = GameObject.Find("doomguy2/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, InteractRange));
        Vector3 direction = target - cam.Cam.position;
        Ray ray = new Ray(cam.Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractRange))
        {
            if (hit.collider.gameObject.tag == "Door" && Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("Door").GetComponent<AnimateDoor>().IsOpen = true;
                GameObject.Find("Door").GetComponent<AnimateDoor>().closed = false;

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ShotGunPickUp")
        {
            Destroy(other.gameObject);
            IsWeapon = true;
            WeaponArm1.SetActive(true);
            WeaponArm2.SetActive(true);
            Arm1.SetActive(false);
            Arm2.SetActive(false);
            WeaponId = 1;
            animator.SetBool("IsWeapon", true);
            ShotgunSpentAmmo = Weapon.ShotgunMaxAmmo;
            SetPositionShotgun();
            GameObject CurrentWeapon = Instantiate(Weapons[WeaponId], WeaponLocation.position, Quaternion.identity);
            CurrentWeapon.transform.parent = WeaponLocation;
            CurrentWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            CurrentWeapon.SetActive(true);
            if (FirstWeaponSlot == -1)
            {
                FirstWeaponSlot = 1;
                FirstWSlot = CurrentWeapon;
            }
            else
            {
                foreach (Transform C in GameObject.Find("doomguy2/Main Camera/WeaponLocation").transform)
                {
                    if (C.gameObject.tag == "PlasmaRifle")
                    {
                        C.gameObject.SetActive(false);
                    }
                }
                SecondWSlot = CurrentWeapon;
                SecondWeaponSlot = 1;
                PlasmaRifleSlot.SetActive(false);
            }
            ShotgunSlot.SetActive(true);
        }
        if (other.tag == "PlasmaRiflePickUp")
        {
            Destroy(other.gameObject);
            IsWeapon = true;
            WeaponArm1.SetActive(true);
            WeaponArm2.SetActive(true);
            Arm1.SetActive(false);
            Arm2.SetActive(false);
            WeaponId = 0;

            PlasmarifleSpentAmmo = Weapon.PlasmarifleMaxAmmo;
            SetPositionPlasmarifle();
            GameObject CurrentWeapon = Instantiate(Weapons[WeaponId], WeaponLocation.position, Quaternion.identity);
            CurrentWeapon.transform.parent = WeaponLocation;
            CurrentWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            CurrentWeapon.SetActive(true);
            if (FirstWeaponSlot == -1)
            {
                FirstWeaponSlot = 0;
                FirstWSlot = CurrentWeapon;
            }
            else
            {
                foreach (Transform C in GameObject.Find("doomguy2/Main Camera/WeaponLocation").transform)
                {
                    if (C.gameObject.tag == "ShotGun")
                    {
                        C.gameObject.SetActive(false);
                    }
                }
                SecondWeaponSlot = 0;
                ShotgunSlot.SetActive(false);
                SecondWSlot = CurrentWeapon;
            }
            PlasmaRifleSlot.SetActive(true);
        }
        if (other.tag == "Key")
        {
            Destroy(Key);
            HasKey = true;
        }
        if (other.tag == "PlasmaRifleAmmo")
        {
            if (PlasmarifleAmmo + 30 <= Weapon.PlasmarifleMaxClip)
            {
                Destroy(other.gameObject);
                PlasmarifleAmmo += 30;
            }
            else
            {
                Destroy(other.gameObject);
                PlasmarifleAmmo = Weapon.PlasmarifleMaxClip;
            }
        }
        if (other.tag == "ShotGunAmmo")
        {
            if (ShotgunAmmo + 12 <= Weapon.ShotgunMaxClip)
            {
                Destroy(other.gameObject);
                ShotgunAmmo += 12;
            }
            else
            {
                Destroy(other.gameObject);
                ShotgunAmmo = Weapon.ShotgunMaxClip;
            }
        }

    }
    public IEnumerator ReloadPlasmarifle()
    {
        CanReloadPlasmarifle = false;
        if (WeaponId == 0)
        {
            GetComponent<AudioSource>().clip = ShootSounds[5];
            GetComponent<AudioSource>().volume = 1f;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(Weapon.PlasmarifleReloadDelay);
            if (PlasmarifleSpentAmmo <= 0)
            {
                if (PlasmarifleAmmo >= Weapon.PlasmarifleMaxAmmo)
                {
                    PlasmarifleAmmo -= Weapon.PlasmarifleMaxAmmo;
                    PlasmarifleSpentAmmo = Weapon.PlasmarifleMaxAmmo;
                }
                else
                {
                    PlasmarifleSpentAmmo = PlasmarifleAmmo;
                    PlasmarifleAmmo = 0;
                }
            }
            else
            {
                if (PlasmarifleAmmo >= Weapon.PlasmarifleMaxAmmo)
                {
                    PlasmarifleAmmo = PlasmarifleAmmo - Weapon.PlasmarifleMaxAmmo + PlasmarifleSpentAmmo;
                    PlasmarifleSpentAmmo = Weapon.PlasmarifleMaxAmmo;
                }
                else
                {
                    PlasmarifleSpentAmmo += PlasmarifleAmmo;
                    PlasmarifleAmmo = 0;
                }
            }
        }
        CanShootPlasmarifle = true;
        ReloadText.enabled = false;
        CanReloadPlasmarifle = true;
    }
    public IEnumerator ReloadShotgun()
    {
        CanReloadShotgun = false;
        if (WeaponId == 1)
        {
            if (ShotgunSpentAmmo <= 0f)
            {
                if (ShotgunAmmo >= Weapon.ShotgunMaxAmmo)
                {
                    for (int i = 0; i < Weapon.ShotgunMaxAmmo; i++)
                    {
                        GetComponent<AudioSource>().clip = ShootSounds[4];
                        GetComponent<AudioSource>().volume = 1f;
                        GetComponent<AudioSource>().Play();
                        yield return new WaitForSeconds(Weapon.ShotgunReloadDelay);
                        ShotgunSpentAmmo++;
                        ShotgunAmmo--;
                    }
                }
                else
                {
                    for (int i = 0; i < ShotgunAmmo; i++)
                    {
                        GetComponent<AudioSource>().clip = ShootSounds[4];
                        GetComponent<AudioSource>().volume = 1f;
                        GetComponent<AudioSource>().Play();
                        yield return new WaitForSeconds(Weapon.ShotgunReloadDelay);
                        ShotgunSpentAmmo++;
                        ShotgunAmmo--;
                    }
                }
            }
            else
            {
                int a = ShotgunSpentAmmo;
                if (ShotgunAmmo >= Weapon.ShotgunMaxAmmo)
                {
                    for (int i = 0; i < Weapon.ShotgunMaxAmmo - a; i++)
                    {
                        GetComponent<AudioSource>().clip = ShootSounds[4];
                        GetComponent<AudioSource>().volume = 1f;
                        GetComponent<AudioSource>().Play();
                        yield return new WaitForSeconds(Weapon.ShotgunReloadDelay);
                        ShotgunSpentAmmo++;
                        ShotgunAmmo--;
                    }
                }
                else
                {
                    if (ShotgunSpentAmmo + ShotgunAmmo > Weapon.ShotgunMaxAmmo)
                    {
                        for (int i = 0; i < Weapon.ShotgunMaxAmmo - a; i++)
                        {
                            GetComponent<AudioSource>().clip = ShootSounds[4];
                            GetComponent<AudioSource>().volume = 1f;
                            GetComponent<AudioSource>().Play();
                            yield return new WaitForSeconds(Weapon.ShotgunReloadDelay);
                            ShotgunSpentAmmo++;
                            ShotgunAmmo--;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ShotgunAmmo; i++)
                        {
                            GetComponent<AudioSource>().clip = ShootSounds[4];
                            GetComponent<AudioSource>().volume = 1f;
                            GetComponent<AudioSource>().Play();
                            yield return new WaitForSeconds(Weapon.ShotgunReloadDelay);
                            ShotgunSpentAmmo++;
                            ShotgunAmmo--;
                        }
                    }
                }
            }


        }
        CanShootShotgun = true;
        ReloadText.enabled = false;
        CanReloadShotgun = true;


    }
    public void SetPositionPlasmarifle()
    {
        WeaponLocationPos = new Vector3(0.6f, -3.32f, 2.64f);
        WeaponLocationRot = new Vector3(2.154f, -1.04f, -2.262f);
        Arm1Pos = new Vector3(5.208f, -4.125f, 3.594f);
        Arm2Pos = new Vector3(-2.91f, -5.85f, 1.26f);
        Arm1Rot = new Vector3(-26.765f, 240.033f, -117.339f);
        Arm2Rot = new Vector3(-49.883f, 135.787f, -74.017f);
        WeaponLocation.localPosition = WeaponLocationPos;
        WeaponLocation.localEulerAngles = new Vector3(0, 0, 0);
        WeaponLocation.Rotate(WeaponLocationRot);
        WeaponArm1.transform.localPosition = Arm1Pos;
        WeaponArm2.transform.localPosition = Arm2Pos;
        WeaponArm1.transform.localEulerAngles = new Vector3(0, 0, 0);
        WeaponArm2.transform.localEulerAngles = new Vector3(0, 0, 0);
        WeaponArm1.transform.Rotate(Arm1Rot);
        WeaponArm2.transform.Rotate(Arm2Rot);
        ParticlePointPosition = new Vector3(0f, 2.15f, 2.72f);
        ParticlePointRotation = new Vector3(90f, 180f, 0f);
        ParticlePoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        ParticlePoint.localPosition = ParticlePointPosition;
    }
    public void SetPositionShotgun()
    {
        WeaponLocationPos = new Vector3(0.857f, -1.106f, 1.981f);
        WeaponLocationRot = new Vector3(-7.141f, 5.171f, -1.452f);
        Arm1Pos = new Vector3(5.84f, -0.32f, 1.74f);
        Arm2Pos = new Vector3(-3.04f, -4f, 2.89f);
        Arm1Rot = new Vector3(3f, 254.313f, -128.574f);
        Arm2Rot = new Vector3(-16.819f, 143.624f, -100.755f);
        WeaponLocation.localPosition = WeaponLocationPos;
        WeaponLocation.localEulerAngles = new Vector3(0, 0, 0);
        WeaponLocation.Rotate(WeaponLocationRot);
        WeaponArm1.transform.localPosition = Arm1Pos;
        WeaponArm2.transform.localPosition = Arm2Pos;
        WeaponArm1.transform.localEulerAngles = new Vector3(0, 0, 0);
        WeaponArm2.transform.localEulerAngles = new Vector3(0, 0, 0);
        WeaponArm1.transform.Rotate(Arm1Rot);
        WeaponArm2.transform.Rotate(Arm2Rot);
        ParticlePointPosition = new Vector3(0f, 0.359f, 2.4472f);
        ParticlePoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        ParticlePoint.localPosition = ParticlePointPosition;
    }
    public void SetZeroPosition()
    {
        if (WeaponLocation.childCount == 2)
        {
            WeaponLocation.GetChild(0).position = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(1).position = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(0).localPosition = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(1).localPosition = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(0).eulerAngles = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(1).eulerAngles = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            WeaponLocation.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void MoveCamera()
    {
        GameObject.Find("BlackDemon").GetComponent<BlackDemon>().StartCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        GameObject.Find("BlackDemon").GetComponent<BlackDemon>().CutScene.GetComponent<PlayableDirector>().enabled = false;
    }
    public void AnimationsOff()
    {
        animator.SetBool("EndGame", true);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSprinting", false);
        animator.SetBool("IsJumped", false);
        animator.SetBool("IsWeapon", false);
    }
    public void VictoryAnimation()
    {

        if (WeaponId == 0)
        {
            VictoryPlasmaRifle.SetActive(true);
            Debug.Log(VictoryPlasmaRifle.activeInHierarchy);
        }
        else if (WeaponId == 1)
        {
            VictoryShotgun.SetActive(true);
        }
        animator.SetBool("Victory", true);
    }
    public void HideWeapons()
    {
        if (WeaponId == 0)
        {
            VictoryPlasmaRifle.SetActive(false);
        }
        else if (WeaponId == 1)
        {
            VictoryShotgun.SetActive(false);
        }
    }        
}
    


