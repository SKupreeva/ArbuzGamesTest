using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// class controls UI changing, inits button's click listeners, enabling/disabling end game panels

public class UIController : MonoBehaviour
{
    [Header("Game components")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Timer _timer;

    [Space][Header("Joystick")]
    [SerializeField] private FloatingJoystick _joystick;
    
    [Space][Header("Text")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private TextMeshProUGUI _looserText;
    [SerializeField] private TextMeshProUGUI _winnerText;

    [Space][Header("Retry panel")]
    [SerializeField] private RectTransform _retryPanel;

    [Space][Header("Buttons")]
    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;

    private void OnEnable()
    {
        _timer.OnTimerValueChanged += () => DisplayTime(_timer.TimerCurrentValue);
        _levelManager.OnCoinCollected += () => DisplayCoins(_levelManager.CoinsCollected, _levelManager.CoinsToCollect);
        _levelManager.OnGameEnded += StopGame;
        _levelManager.OnNewGameStarted += OnNewGameStarted;

        _yesBtn.onClick.AddListener(RestartGame);
        _noBtn.onClick.AddListener(Application.Quit);
    }

    private void Start()
    {
        DisplayTime(_timer.TimerCurrentValue);
        DisplayCoins(_levelManager.CoinsCollected, _levelManager.CoinsToCollect);
    }

    private void StopGame()
    {
        _winnerText.gameObject.SetActive(_levelManager.CoinsCollected >= _levelManager.CoinsToCollect);
        _looserText.gameObject.SetActive(_levelManager.CoinsCollected < _levelManager.CoinsToCollect);
        _retryPanel.gameObject.SetActive(true);
        
        _joystick.gameObject.SetActive(false);
    }

    private void OnNewGameStarted()
    {
        _winnerText.gameObject.SetActive(false);
        _looserText.gameObject.SetActive(false);
        _retryPanel.gameObject.SetActive(false);

        _joystick.gameObject.SetActive(true);
        _joystick.OnPointerUp(null);

        DisplayTime(_timer.TimerCurrentValue);
        DisplayCoins(_levelManager.CoinsCollected, _levelManager.CoinsToCollect);
    }

    private void RestartGame()
    {
        _levelManager.StartNewGame();
    }

    private void DisplayCoins(int current, int toCollect)
    {
        _coinsCountText.text = string.Format("{0}/{1}", current, toCollect);
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
