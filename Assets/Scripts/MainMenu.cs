using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator SettingsBtnAnimator;

    public Text LevelText;

    public Image AudioImg, VibrationImg;

    public Sprite AudioON, AudioOFF, VibrationON, VibrationOFF;

    private void Start()
    {
        LevelText.text = "Lvl " + GameManager.instance.CurrentLevel.ToString();
    }

    public void SettingsBtn()
    {
        SettingsBtnAnimator.SetBool("open",!SettingsBtnAnimator.GetBool("open"));
        string s = SettingsBtnAnimator.GetBool("open") == true ? "panelopen" : "panelclose";
        AudioManager.instance.Play(s);
    }

    public void ToggleAudio(int n)
    {
        AudioImg.sprite = n == 1 ? AudioON : AudioOFF;
    }

    public void ToggleVibration(int n)
    {
        VibrationImg.sprite = n == 1 ? VibrationON : VibrationOFF;
    }
}
