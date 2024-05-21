using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float sensitivity = 200f;
    public Transform Cam;
    public Vector3 camRotation;
    private float minAngle = -30f;
    private float maxAngle = 60f;
    public bool IsRunning = false;
    public Vector3 StartPosition;
    public Player P;
    public bool IsReturning = false;
    public float PreviousX;

    private void Rotate()
    {
        transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        if (!IsReturning)
        {
            camRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            camRotation.x = Mathf.Clamp(camRotation.x, minAngle, maxAngle);
        }
        else
        {
            RecoilReturn();
        }
        Cam.localEulerAngles = camRotation;
    }    
    void Start()
    {
        StartPosition = Cam.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        P = GameObject.Find("doomguy2").GetComponent<Player>();
        P.PlayerRot = transform.localRotation;
        P.CameraRot = Cam.localRotation;
    }
    void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead && !GameObject.Find("doomguy2").GetComponent<Player>().IsKeyPad)
        {
            Rotate();
            Ray ray = new Ray(Cam.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.1f))
            {
                if (hit.transform != null)
                {
                    Vector3 pos = new Vector3(StartPosition.x, StartPosition.y, StartPosition.z);
                    Cam.transform.localPosition += (pos - Cam.transform.localPosition) * Time.deltaTime * 2;
                }
            }
        }
    }
    public void RecoilReturn()
    {
        if(Cam.localEulerAngles.x  < PreviousX)
        {
            camRotation.x = 7f;
        }
        else
        {
            IsReturning = false;
        }
    }
}