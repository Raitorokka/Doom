//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Audio;

public class UI : MonoBehaviour
{
    public Text WalkForwardT;
    public Text WalkBackwardT;
    public Text WalkLeftT;
    public Text WalkRightT;
    public Text JumpT;
    public Text ShootT;
    public Text Slot1T;
    public Text Slot2T;
    public KeyBoardManager KeyBoardManager;
    public Image WalkForwardI;
    public Image WalkBackwardI;
    public Image WalkLeftI;
    public Image WalkRightI;
    public Image JumpI;
    public Image ShootI;
    public Image Slot1I;
    public Image Slot2I;
    public Toggle ShowFps;
    public List<Image> KeyButtons;
    public List<KeyCode> repKeys;
    public Text Error;
    public GameObject Graphics;
    public GameObject Controls;
    public GameObject Audio;
    public GameObject FPSCounter;
    public AudioMixerGroup Mixer;
    public Slider VolumeSlider;
    public Slider MusicSlider;
    public Slider SoundsSlider;
    public AudioSettings AudioSettings;
    public SettingsData settings;
    public Text Back;
    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider = GameObject.Find("Canvas/Audio/Main Volume").GetComponent<Slider>();
        MusicSlider = GameObject.Find("Canvas/Audio/Music").GetComponent<Slider>();
        SoundsSlider = GameObject.Find("Canvas/Audio/GameSounds").GetComponent<Slider>();
        FPSCounter = GameObject.Find("Canvas/FPSCounter");
        FPSCounter.SetActive(false);
        Audio = GameObject.Find("Audio");
        Graphics = GameObject.Find("Graphics");
        Controls = GameObject.Find("Controls");
        WalkForwardT = GameObject.Find("Canvas/Controls/Scroll/Content/WalkForward/Text (Legacy)").GetComponent<Text>();
        WalkBackwardT = GameObject.Find("Canvas/Controls/Scroll/Content/WalkBackward/Text (Legacy)").GetComponent<Text>();
        WalkLeftT = GameObject.Find("Canvas/Controls/Scroll/Content/WalkLeft/Text (Legacy)").GetComponent<Text>();
        WalkRightT = GameObject.Find("Canvas/Controls/Scroll/Content/WalkRight/Text (Legacy)").GetComponent<Text>();        
        JumpT = GameObject.Find("Canvas/Controls/Scroll/Content/Jump/Text (Legacy)").GetComponent<Text>();
        ShootT = GameObject.Find("Canvas/Controls/Scroll/Content/Shoot/Text (Legacy)").GetComponent<Text>();
        Slot1T = GameObject.Find("Canvas/Controls/Scroll/Content/Slot1/Text (Legacy)").GetComponent<Text>();
        Slot2T = GameObject.Find("Canvas/Controls/Scroll/Content/Slot2/Text (Legacy)").GetComponent<Text>();
        KeyBoardManager = GameObject.Find("KeyBoardManager").GetComponent<KeyBoardManager>();
        KeyBoardManager.enabled = false;
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/WalkForward").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/WalkBackward").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/WalkLeft").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/WalkRight").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/Jump").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/Shoot").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/Slot1").GetComponent<Image>());
        KeyButtons.Add(GameObject.Find("Canvas/Controls/Scroll/Content/Slot2").GetComponent<Image>());
        ShowFps = GameObject.Find("Canvas/Graphics/ShowFps").GetComponent<Toggle>();
        Error = GameObject.Find("Canvas/Error").GetComponent<Text>();    
        Audio.SetActive(false);
        Graphics.SetActive(false);
        Controls.SetActive(false);                
        VolumeSlider.value = AudioSettings.MasterVolumeSound;
        MusicSlider.value = AudioSettings.MasterMusicSound;
        SoundsSlider.value = AudioSettings.MasterGameSound;
        Back = GameObject.Find("Canvas/Back/Text").GetComponent<Text>();
        if(settings.PreviousSceneName == "MainMenu")
        {
            Back.text = "Back to main menu";
        }
        else if(settings.PreviousSceneName == "L1" || settings.PreviousSceneName == "L2")
        {
            Back.text = "Continue";
        }
    }    
    void Update()
    {
        
        WalkForwardT.text = Info.WForward.ToString();
        WalkBackwardT.text = Info.WBackward.ToString();
        WalkLeftT.text = Info.WLeft.ToString();
        WalkRightT.text = Info.WRight.ToString();
        ShootT.text = Info.Shoot.ToString();
        JumpT.text = Info.Jump.ToString();
        Slot1T.text = Info.Slot1.ToString();
        Slot2T.text = Info.Slot2.ToString(); 
    }
    public void CheckIsRed()
    {
        KeyCode[] Keys = { Info.WForward, Info.WBackward, Info.WLeft, Info.WRight, Info.Jump, Info.Shoot, Info.Slot1, Info.Slot2 };
        if (repKeys.Count != 0)
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                foreach (KeyCode J in repKeys)
                {
                    KeyButtons[i].color = Color.white;
                    if (J == Keys[i])
                    {
                        KeyButtons[i].color = Color.red;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (Image b in KeyButtons)
            {
                b.color = Color.white;
            }
        }
    }
    public void CheckButton()
    {
        repKeys.Clear();
        KeyCode[] keys = { Info.WForward, Info.WBackward, Info.WLeft, Info.WRight, Info.Jump, Info.Shoot, Info.Slot1, Info.Slot2 };
        for(int i = 0; i < keys.Length - 1; i++)
        {
            for(int j = i + 1; j < keys.Length; j++)
            {
                if (keys[i] == keys[j] && !repKeys.Contains(keys[i]))
                {
                    repKeys.Add(keys[i]);
                }
            }
        }
    }
    public void ChangeKey(int name)
    {
        KeyBoardManager.enabled = true;
        KeyBoardManager.CurrentButton = name;
    }
    public void BackToMenu()
    {
        if (CanExit())
        {
            SceneManager.UnloadScene("Settings");
        }
        else
        {
            GameObject.Find("Canvas/Error").GetComponent<TextAnimation>().Show = true;            
        }
    }
    
    public void Respawn()
    {
        if (CanExit())
        {
            SceneManager.LoadScene("L1");
        }
        else
        {
            GameObject.Find("Canvas/Error").GetComponent<TextAnimation>().Show = true;
        }
    }
    public bool CanExit() 
    {
        return KeyButtons.All(x => x.color != Color.red);
    }
    public void ShowGraphics()
    {
        Graphics.SetActive(true);
        Controls.SetActive(false);
        Audio.SetActive(false);
    }
    public void ShowControls()
    {
        Graphics.SetActive(false);
        Controls.SetActive(true);
        Audio.SetActive(false);
    }
    public void ShowAudio()
    {
        Graphics.SetActive(false);
        Controls.SetActive(false);
        Audio.SetActive(true);
    }
    public void ChangeFPS(int Number)
    {
        Application.targetFrameRate = Number;
    }
    public void ToggleFPS()
    {        
        Settings.FPS = ShowFps.isOn;
    }
    public void ChangeMasterVolume()
    {
        Mixer.audioMixer.SetFloat("MasterVolume", VolumeSlider.value);
        Mixer.audioMixer.GetFloat("MasterVolume", out AudioSettings.MasterVolumeSound);
    }
    public void ChangeMusicVolume()
    {
        Mixer.audioMixer.SetFloat("MasterMusic", MusicSlider.value);
        Mixer.audioMixer.GetFloat("MasterMusic", out AudioSettings.MasterMusicSound);
    }
    public void ChangeGameSoundsVolume()
    {
        Mixer.audioMixer.SetFloat("MasterSound", SoundsSlider.value);
        Mixer.audioMixer.GetFloat("MasterSound", out AudioSettings.MasterGameSound);
    }
}
