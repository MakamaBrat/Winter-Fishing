using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingGameManager : MonoBehaviour
{
    public AudioSource ui;
    [Header("Buttons")]
    public GameObject castButton;       // "Забросить удочку"
    public GameObject pullButton;       // "Подсечь!"
    public GameObject touchButton;      // "Нажать!" в мини-игре


    [Header("Slider Mini Game")]
    public Image slider;
    public TextMeshProUGUI fastValueText;
    [Range(0.1f, 5f)] public float sliderSpeed = 1.8f;

    [Header("Catch Settings")]
    [Range(0f, 1f)] public float successMin = 0.42f;
    [Range(0f, 1f)] public float successMax = 0.58f;

    [Header("Titles & Popups")]
    public GameObject fishHookedTitle;
    public GameObject winPopup;
    public GameObject losePopup;

    [Header("Timing")]
    public float minWaitTime = 2.2f;
    public float maxWaitTime = 5.5f;

    private enum GameState { ReadyToCast, WaitingForBite, CanHook, MiniGame, Result }
    private GameState currentState = GameState.ReadyToCast;

    private bool sliderActive = false;
    private float sliderDirection = 1f;
    private float slowValue = 0f;
    public FillImageTriple fillImageTriple;
    public Transform tool0;
    public Transform tool1;

    private void OnEnable()
    {
        ResetGame();
        fillImageTriple.ResetFill();
    }

    private void ResetGame()
    {
        StopAllCoroutines();
        tool0.gameObject.SetActive(true);
        tool1.gameObject.SetActive(false);
        castButton.SetActive(true);
        pullButton.SetActive(false);
        touchButton.SetActive(false);

        slider.gameObject.SetActive(false);
        sliderActive = false;
        sliderDirection = 1f;
        slowValue = 0f;
        slider.fillAmount = 0f;

        winPopup.SetActive(false);
        losePopup.SetActive(false);
        fishHookedTitle.SetActive(false);

        currentState = GameState.ReadyToCast;
    }



    // ── Заброс удочки ───────────────────────────────────────────
    public void OnCastButton()
    {
        
        castButton.SetActive(false);
        pullButton.SetActive(true);
        tool0.gameObject.SetActive(false);
        tool1.gameObject.SetActive(true);
        currentState = GameState.WaitingForBite;
        ui.Play();

        StopAllCoroutines();
        StartCoroutine(WaitForPossibleBite());
    }

    private IEnumerator WaitForPossibleBite()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        
            currentState = GameState.CanHook;
            pullButton.SetActive(false);
        touchButton.gameObject.SetActive(true);
        slider.transform.parent.gameObject.SetActive(true);
           StartMiniGame();
       
        
    }

    // ── Подсечка ─────────────────────────────────────────────────
    public void OnPullButton()
    {


        ui.Play();
        pullButton.SetActive(false);
            castButton.gameObject.SetActive(true);
        tool0.gameObject.SetActive(true);
        tool1.gameObject.SetActive(false);


    }

    private IEnumerator ResetAfterEarlyPull(float delay)
    {
        yield return new WaitForSeconds(delay);
        castButton.SetActive(true);
        pullButton.SetActive(false);
        touchButton.SetActive(false);
        currentState = GameState.ReadyToCast;

    }

    private void StartMiniGame()
    {
        slider.gameObject.SetActive(true);
        fishHookedTitle.SetActive(true);
        touchButton.gameObject.SetActive(true);
        pullButton.SetActive(false);
        castButton.gameObject.SetActive(false);
        sliderActive = true;
        sliderDirection = 1f;
        slowValue = 0f;
        slider.fillAmount = 0f;
        slider.gameObject.SetActive(true);

        touchButton.SetActive(true);
    }

    // ── Действие в мини-игре ─────────────────────────────────────
    public void OnTouchButton()
    {
        if (!sliderActive) return;
        ui.Play();
        sliderActive = false;
        touchButton.SetActive(false);
        slider.gameObject.transform.parent.gameObject.SetActive(false);
        currentState = GameState.Result;
        CheckCatchResult();
    }

    // ── Обновление слайдера ──────────────────────────────────────
    private void Update()
    {
        if (!sliderActive) return;

        slider.fillAmount += Time.deltaTime * sliderSpeed * sliderDirection;

        if (slider.fillAmount >= 1f)
        {
            slider.fillAmount = 1f;
            sliderDirection = -1f;
        }
        else if (slider.fillAmount <= 0f)
        {
            slider.fillAmount = 0f;
            sliderDirection = 1f;
        }

        slowValue = Mathf.MoveTowards(slowValue, slider.fillAmount, Time.deltaTime * sliderSpeed * 0.28f);

        if (fastValueText != null) fastValueText.text = slider.fillAmount.ToString("0.00");
      
    }

    // ── Результат ─────────────────────────────────────────────────
    private void CheckCatchResult()
    {
        fishHookedTitle.SetActive(false);

        bool success = slider.fillAmount >= successMin && slider.fillAmount <= successMax;

        if (success)
        {
            //winPopup.SetActive(true);
            fillImageTriple.PlusOneFill();
        }
        else
        {
            ui.Play();
            losePopup.SetActive(true);
        }

        StartCoroutine(BackToCastAfterDelay(2.8f));
    }

    private IEnumerator BackToCastAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
    }
}
