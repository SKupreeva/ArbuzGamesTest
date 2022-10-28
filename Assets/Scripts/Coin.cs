using UnityEngine;
using UnityEngine.Events;

// class controls coin

public class Coin : MonoBehaviour
{
    public UnityEvent OnCoinCollected;

    public void OnPlayerGetCoin()
    {
        gameObject.SetActive(false);
        OnCoinCollected?.Invoke();
    }
}
