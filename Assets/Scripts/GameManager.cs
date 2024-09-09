using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool GameStarted = false;

    //FOR UI ELEMENTS
    public GameObject Tutorial;
    public GameObject MainMenuPanel;
    public GameObject InGamePanel;
    public GameObject GameWonPanel;
    public GameObject GameOverPanel;
    public Button PlayBtn;
    [HideInInspector]
    public Transform unspawnedMainCube;
    GameObject FinalPSPrefab;

    [HideInInspector]
    public int CurrentLevel = 1, Audio, Vibration;
    //[HideInInspector]
    public Transform currentLvlHodler, currentTargetsHolder;

    public Transform MainCubeHolder;
    public GameObject[] lvls;

    int currentMainAudio = 1;
    public static int previousAudio = 0;

    public bool PlayLevels = false;
    public int PlayableLevel = 5;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        if(PlayerPrefs.HasKey("lvl") == false)
        {
            PlayerPrefs.SetInt("lvl", 1);
        }
        if (PlayLevels == true)
        {
            CurrentLevel = PlayerPrefs.GetInt("lvl");
        }
        else
        {
            PlayerPrefs.SetInt("lvl", PlayableLevel);
            CurrentLevel = PlayableLevel;
        }

        foreach (GameObject g in lvls)
        {
            g.SetActive(false);
        }
        if (CurrentLevel <= 30)
        {
            lvls[CurrentLevel - 1].SetActive(true);
            currentLvlHodler = lvls[CurrentLevel - 1].transform.GetChild(0);
            currentTargetsHolder = lvls[CurrentLevel - 1].transform.GetChild(1);
        }
        else if(CurrentLevel % 30 != 1)
        {
            int c = CurrentLevel % 30;
            lvls[c - 1].SetActive(true);
            currentLvlHodler = lvls[c - 1].transform.GetChild(0);
            currentTargetsHolder = lvls[c - 1].transform.GetChild(1);
        }
        else
        {
            int c2 = 4;
            lvls[c2 - 1].SetActive(true);
            currentLvlHodler = lvls[c2 - 1].transform.GetChild(0);
            currentTargetsHolder = lvls[c2 - 1].transform.GetChild(1);
        }

        StartSetter();
    }

    private void Start()
    {
        FinalPSPrefab = Resources.Load<GameObject>("FinalPS");

        currentMainAudio = Random.Range(1, 3);
        if (currentMainAudio != previousAudio)
        {
            if (previousAudio != 0)
            {
                AudioManager.instance.Stop("maintheme" + previousAudio);
            }
            AudioManager.instance.Play("maintheme" + currentMainAudio);
        }
        else
        {
            AudioManager.instance.SetVolume("maintheme" + currentMainAudio, .7f);
        }
        previousAudio = currentMainAudio;
    }


    void StartSetter()
    {
        if(PlayerPrefs.HasKey("Audio") == false)        //For Audio Starts Setting
        {
            PlayerPrefs.SetInt("Audio", 1);
        }
        if(PlayerPrefs.HasKey("Vibration") == false)
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }
        Audio = PlayerPrefs.GetInt("Audio");
        Vibration = PlayerPrefs.GetInt("Vibration");
        MainMenuPanel.GetComponent<MainMenu>().ToggleAudio(Audio);
        MainMenuPanel.GetComponent<MainMenu>().ToggleVibration(Vibration);
        //Camera.main.GetComponent<AudioListener>().enabled = Audio == 1 ? true : false;       //For Audio Start Setting Ends
        AudioListener.pause = Audio == 1 ? false : true;

        GameStarted = false;
        MainMenuPanel.SetActive(true);
        InGamePanel.SetActive(false);
        Tutorial.SetActive(false);
        GameWonPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ReloadScene();
        }
    }

    public void StartGame()
    {
        GameStarted = true;
        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        Tutorial.SetActive(CurrentLevel == 1 ? true : false);
        if (unspawnedMainCube != null)
        {
            unspawnedMainCube.GetComponent<MainCube>().SpawnHollow();
        }
        Physics.IgnoreLayerCollision(0, 1, false);
        Physics.IgnoreLayerCollision(1, 1, true);
        AudioManager.instance.Play("click");
    }

    public void ReloadScene()
    {
        Physics.IgnoreLayerCollision(0, 1, true);
        currentLvlHodler.GetComponent<GroundCubesAssembler>().ReturnCubes();
        StartCoroutine(reload());
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StageFinished()
    {
        //if(PlayerPrefs.GetInt("lvl") < lvls.Length)
        //{
        //    PlayerPrefs.SetInt("lvl", PlayerPrefs.GetInt("lvl") + 1);
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("lvl", 2);
        //}
        PlayerPrefs.SetInt("lvl", PlayerPrefs.GetInt("lvl") + 1);

        InGamePanel.SetActive(false);
        GameWonPanel.SetActive(true);
        GameStarted = false;

        for(int i = 0; i < MainCubeHolder.childCount; i ++)
        {
            Instantiate(FinalPSPrefab, MainCubeHolder.GetChild(i).position, FinalPSPrefab.transform.rotation);
        }
        AudioManager.instance.SetVolume("maintheme" + currentMainAudio, .03f);
        AudioManager.instance.Play("victory");
        //AudioManager.instance.Stop("validate");

        Camera.main.GetComponent<CameraMovement>().SpawnPartyPapers();
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        GameStarted = false;
        AudioManager.instance.SetVolume("maintheme" + currentMainAudio, .08f);
        AudioManager.instance.Play("gameover");
        AudioManager.instance.Stop("validate");

        for(int i = 0; i < MainCubeHolder.childCount; i++)
        {
            MainCubeHolder.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        Physics.IgnoreLayerCollision(1, 1, false);
    }

    public void ToggleAudio()
    {
        Audio = PlayerPrefs.GetInt("Audio") == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Audio", Audio);
        MainMenuPanel.GetComponent<MainMenu>().ToggleAudio(Audio);
        //Camera.main.GetComponent<AudioListener>().enabled = Audio == 1 ? true : false;
        AudioListener.pause = Audio == 1 ? false : true;
    }

    public void ToggleVibration()
    {
        Vibration = PlayerPrefs.GetInt("Vibration") == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Vibration", Vibration);
        MainMenuPanel.GetComponent<MainMenu>().ToggleVibration(Vibration);
    }

    public void setPlayBtn()
    {
        PlayBtn.interactable = true;
    }
}
