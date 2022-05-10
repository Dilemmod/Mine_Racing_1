using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volume;
    [SerializeField] private AudioMixer masterMixer;
    private float curretVolume;

    [Header("Quality")]
    [SerializeField] Dropdown qualityDropdown;
    private string[] qualityLevels;
    #region Singleton
    public static SettingsMenuController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion
    void Start()
    {
        volume.onValueChanged.AddListener(OnVolumeChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        volume.value = GetVolume();
        FullerQualityDropdown();
    }
    private void OnDestroy()
    {
        volume.onValueChanged.RemoveListener(OnVolumeChanged);
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
    }
    #region Volume
    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("PlayerVolume", volume);
        masterMixer.SetFloat("Volume", volume);
    }
    public float GetVolume()
    {
        masterMixer.GetFloat("Volume", out curretVolume);
        return curretVolume;
    }
    private void OnVolumeChanged(float volume)
    {
        SetVolume(volume);
    }
    #endregion
    #region Quality
    public int GetQuality()
    {
        return QualitySettings.GetQualityLevel();
    }

    //[System.Obsolete]
    public void SetQuality(int index)
    {
        void SerProfileSettings(bool active)
        {
            PostProcessProfile post =
            (PostProcessProfile)Resources.Load("Material/PostProcessing/Profile");
            post.GetSetting<Bloom>().active = active;
            post.GetSetting<AmbientOcclusion>().active = active;
            post.GetSetting<ColorGrading>().active = active;
        }
        //Materials
        Material[] materials = Resources.LoadAll<Material>("Material");
        switch (index)
        {
            case 0:
                SerProfileSettings(false);
                for (int i = 0; i < materials.Length; i++)
                    materials[i].shader = Shader.Find("Mobile/Diffuse");
                break;
            case 1:
                SerProfileSettings(false);
                for (int i = 0; i < materials.Length; i++)
                    materials[i].shader = Shader.Find("Mobile/Diffuse");
                break;
            case 2:
                SerProfileSettings(true);
                for (int i = 0; i < materials.Length; i++)
                    materials[i].shader = Shader.Find("Nature/Tree Creator Leaves");
                break;
        }
        //Disable objects
        foreach (var gameObject in FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (gameObject.layer == 13 || gameObject.layer == 14)
            {
                switch (index)
                {
                    case 0:
                        gameObject.SetActive(false);
                        break;
                    case 1:
                        if (gameObject.layer == 13)
                            gameObject.SetActive(true);
                        else if (gameObject.layer == 14)
                            gameObject.SetActive(false);
                        break;
                    case 2:
                        if (gameObject.layer == 13)
                            gameObject.SetActive(true);
                        else if (gameObject.layer == 14)
                            gameObject.SetActive(true);
                        break;
                }
            }
        }
        PlayerPrefs.SetInt("PlayerQuality", index);
        QualitySettings.SetQualityLevel(index, true);
    }
    private void FullerQualityDropdown()
    {
        //Full Dropdown
        qualityLevels = QualitySettings.names;
        for (int i = 0; i < qualityLevels.Length; i++)
            qualityLevels[i] = qualityLevels[i].ToUpper();
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityLevels.ToList());

        //Set value Dropdown
        qualityDropdown.value = GetQuality();
        qualityDropdown.RefreshShownValue();
        //serQuality
    }
    private void OnQualityChanged(int qualityLvl)
    {
        QualitySettings.SetQualityLevel(qualityLvl,true);
        SetQuality(qualityLvl);
    }
    #endregion
}
