using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitcher : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    [Header("UI Images")]
    public Image musicImage;
    public Image sfxImage;
    public Image dummyImage; // третья настройка

    [Header("Sprites")]
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("States")]
    public bool musicOn = true;
    public bool sfxOn = true;
    public bool dummyOn = true; // третья настройка

    [Header("Transparency Settings")]
    [Range(0f, 1f)] public float offAlpha = 0.5f; // прозрачность, когда выключено
    [Range(0f, 1f)] public float onAlpha = 1f;    // прозрачность, когда включено

    private void Start()
    {
        ApplyMusicState();
        ApplySfxState();
        ApplyDummyState();
    }

    // --------------------
    // SWITCHERS
    // --------------------

    public void SwitchMusic()
    {
        musicOn = !musicOn;
        ApplyMusicState();
    }

    public void SwitchSfx()
    {
        sfxOn = !sfxOn;
        ApplySfxState();
    }

    public void SwitchDummy()
    {
        dummyOn = !dummyOn;
        ApplyDummyState();
    }

    // --------------------
    // APPLY STATES
    // --------------------

    void ApplyMusicState()
    {
        if (musicSource != null)
            musicSource.mute = !musicOn;

        ApplyImageState(musicImage, musicOn);
    }

    void ApplySfxState()
    {
        foreach (AudioSource sfx in sfxSources)
        {
            if (sfx != null)
                sfx.mute = !sfxOn;
        }

        ApplyImageState(sfxImage, sfxOn);
    }

    void ApplyDummyState()
    {
        ApplyImageState(dummyImage, dummyOn);
    }

    // --------------------
    // HELPER
    // --------------------
    void ApplyImageState(Image img, bool state)
    {
        if (img == null) return;

        // прозрачность
        Color c = img.color;
        c.a = state ? onAlpha : offAlpha;
        img.color = c;

        // спрайт
        if (onSprite != null && offSprite != null)
        {
            img.sprite = state ? onSprite : offSprite;
            img.SetNativeSize();
        }
    }
}
