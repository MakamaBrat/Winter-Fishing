using UnityEngine;
using UnityEngine.UI;

public class ActivateSuccess : MonoBehaviour
{

    public Image sp;
    public Sprite[] sprites;
    public int[] reward;
    public AudioSource au;
    public CoinManager coinManager;

    private void OnEnable()
    {
        int k = Random.Range(0,3);
        coinManager.AddCoins(reward[k]);
        sp.sprite = sprites[k];
        sp.SetNativeSize();
        au.Play();
    }
}
