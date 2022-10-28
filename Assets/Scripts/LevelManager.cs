using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// class controls level generation, start/end game functions

public class LevelManager : MonoBehaviour
{
    [Space][Header("Game components")]
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private CharacterController _player;
    [SerializeField] private Timer _timer;

    [Space][Header("Map components")]
    [SerializeField] private Transform _coinSpawnParent;
    [SerializeField] private Renderer _floor;
    [SerializeField] private RectTransform _canvas;

    public Action OnGameEnded;
    public Action OnNewGameStarted;
    public Action OnCoinCollected;

    public int CoinsToCollect => _coins.Count;

    public int CoinsCollected
    {
        get => _coinsCollectedCount;
        private set
        {
            if (value == _coinsCollectedCount)
            {
                return;
            }
            _coinsCollectedCount = value;
            OnCoinCollected?.Invoke();

            if(value >= CoinsToCollect)
            {
                StopGame();
            }
        }
    }

    private List<Coin> _coins = new List<Coin>();

    private int _coinsCollectedCount;

    private float _mapHalfWidth => _floor.bounds.size.x / 2;
    private float _mapHalfHeight => _floor.bounds.size.z / 2;

    private float _coinSize;

    private void OnEnable()
    {
        if(_coinPrefab.TryGetComponent<Transform>(out var transform))
        {
            _coinSize = transform.localScale.y;
        }

        Init();
        _timer.OnTimerEnded += StopGame;
    }

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        _player.enabled = true;
        CoinsCollected = 0;

        foreach (var coin in _coins)
        {
            coin.OnCoinCollected.RemoveAllListeners();
            coin.transform.position = Vector3.zero;
            coin.gameObject.SetActive(true);
        }

        foreach (var coin in _coins)
        {
            coin.transform.position = GetRandomPositionOnMap(_coinSize * 0.5f);
            coin.OnCoinCollected.AddListener(() => CoinsCollected++);
        }

        _player.Move(GetRandomPositionOnMap(0));
        OnNewGameStarted?.Invoke();
    }

    private void StopGame()
    {
        _player.enabled = false;
        OnGameEnded?.Invoke();
    }

    private void Init()
    {
        var canvasCorners = new Vector3[4];
        _canvas.GetWorldCorners(canvasCorners);

        var floorSquare = _mapHalfWidth * _mapHalfHeight * 4;
        var canvasSquare = Vector3.Distance(canvasCorners[1], canvasCorners[0]) * Vector3.Distance(canvasCorners[1], canvasCorners[2]);

        var count = (int)(floorSquare / canvasSquare);

        for (var i = 0; i < count; i++)
        {
            var coin = Instantiate(_coinPrefab, _coinSpawnParent);
            _coins.Add(coin);
        }
    }

    private Vector3 GetRandomPositionOnMap(float height)
    {
        var newPos = Vector3.zero;

        var count = 0;

        while (count < _coins.Count)
        {
            count = 0;
            newPos = new Vector3(Random.Range(_coinSize - _mapHalfWidth, _mapHalfWidth - _coinSize), height, Random.Range(_coinSize - _mapHalfHeight, _mapHalfHeight - _coinSize));

            foreach (var coin in _coins)
            {
                if (Vector3.Distance(coin.transform.position, newPos) > _coinSize * 2)
                {
                    count++;
                }
            }
        }

        return newPos;
    }
}
