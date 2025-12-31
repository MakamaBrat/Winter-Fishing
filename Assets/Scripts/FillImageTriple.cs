using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FillImageTriple : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;

    [Header("Settings")]
    [SerializeField] private float fillDuration = 0.4f;
    [SerializeField] private int fillCount = 3;

    [Header("Refs")]
    public FishingGameManager gameManager;

    private int currentFill;
    private Coroutine fillCoroutine;
    public AudioSource au;
    private void OnEnable()
    {
        ResetFill();
    }

    // ─────────────── PUBLIC ───────────────
    /// <summary>
    /// ВЫЗЫВАЙ ИЗ ANIMATION EVENT
    /// Каждый вызов = заполнение одной части
    /// </summary>
    public void PlusOneFill()
    {
        au.Play();
        if (fillCoroutine != null)
            return;

        if (currentFill >= fillCount)
            return;

        fillCoroutine = StartCoroutine(FillPartRoutine());

    }

    // ─────────────── CORE ───────────────
    private IEnumerator FillPartRoutine()
    {
        float startFill = (float)currentFill / fillCount;
        float endFill = (float)(currentFill + 1) / fillCount;

        float timer = 0f;

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, timer / fillDuration);
            yield return null;
        }

        fillImage.fillAmount = endFill;
        currentFill++;
        fillCoroutine = null;

        if (currentFill >= fillCount)
            OnFinish();
    }

    // ─────────────── FINISH ───────────────
    private void OnFinish()
    {
        Debug.Log("FILL COMPLETED ✅");

        if (gameManager != null)
        {
            gameManager.winPopup.SetActive(true);
            ResetFill();
        }
    }

    // ─────────────── RESET ───────────────
    public void ResetFill()
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        fillCoroutine = null;
        currentFill = 0;

        if (fillImage != null)
            fillImage.fillAmount = 0f;
    }
}
