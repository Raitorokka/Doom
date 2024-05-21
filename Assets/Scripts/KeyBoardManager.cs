
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardManager : MonoBehaviour
{
    public int CurrentButton;
    public void OnGUI()
    {
        KeyCode[] mouseKeys = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6, };
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                switch (CurrentButton)
                {
                    case 0:
                        {
                            Info.WForward = mouseKeys[i];
                            break;
                        }
                    case 1:
                        {
                            Info.WBackward = mouseKeys[i];
                            break;
                        }
                    case 2:
                        {
                            Info.WLeft = mouseKeys[i];
                            break;
                        }
                    case 3:
                        {
                            Info.WRight = mouseKeys[i];
                            break;
                        }
                    case 4:
                        {
                            Info.Jump = mouseKeys[i];
                            break;
                        }
                    case 5:
                        {
                            Info.Shoot = mouseKeys[i];
                            break;
                        }
                    case 6:
                        {
                            Info.Slot1 = mouseKeys[i];
                            break;
                        }
                    case 7:
                        {
                            Info.Slot2 = mouseKeys[i];
                            break;
                        }
                }
                GameObject.Find("Canvas").GetComponent<UI>().CheckButton();
                GameObject.Find("Canvas").GetComponent<UI>().CheckIsRed();
                enabled = false;
            }
        }
        if (Input.anyKey && Event.current.keyCode != KeyCode.None)
        {
            switch (CurrentButton)
            {
                case 0:
                    {
                        Info.WForward = Event.current.keyCode;
                        break;
                    }
                case 1:
                    {
                        Info.WBackward = Event.current.keyCode;
                        break;
                    }
                case 2:
                    {
                        Info.WLeft = Event.current.keyCode;
                        break;
                    }
                case 3:
                    {
                        Info.WRight = Event.current.keyCode;
                        break;
                    }
                case 4:
                    {
                        Info.Jump = Event.current.keyCode;
                        break;
                    }
                case 5:
                    {
                        Info.Shoot = Event.current.keyCode;
                        break;
                    }
                case 6:
                    {
                        Info.Slot1 = Event.current.keyCode;
                        break;
                    }
                case 7:
                    {
                        Info.Slot2 = Event.current.keyCode;
                        break;
                    }

            }
            GameObject.Find("Canvas").GetComponent<UI>().CheckButton();
            GameObject.Find("Canvas").GetComponent<UI>().CheckIsRed();
            enabled = false;
        }
    }
}
