using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public Slider musicSlider;
    public Slider attackSoundSlider;
    public Slider sensitivitySlider;

    public static float sensitivityValue;
    float sensitivity;
    private void Start()
    {
        musicSlider.value = SoundMgr.Instance.inGameMusic.volume;
        musicSlider.value = SoundMgr.Instance.lobbyMusic.volume;
        attackSoundSlider.value = SoundMgr.Instance.attackSoundVolume;
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1);
        sensitivityValue = sensitivitySlider.value;
    }

    public void MusicVolumeSlider(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        SoundMgr.Instance.inGameMusic.volume = musicSlider.value;
        SoundMgr.Instance.lobbyMusic.volume = musicSlider.value;
        SoundMgr.Instance.musicVolume = musicSlider.value;
    }

    public void AttackSoundVolumeSlider(float volume)
    {
        PlayerPrefs.SetFloat("AttackSoundVolume", attackSoundSlider.value);
        SoundMgr.Instance.attackSoundVolume = attackSoundSlider.value;
    }

    public void SensitivitySlider(float sV)
    {
        sensitivity = sensitivitySlider.value;
        sensitivityValue = sensitivitySlider.value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

}
