using UnityEngine;

// class controls player movement: moves character controller using joystick input
// controls collisions

public class PlayerController : MonoBehaviour
{
    [Space][Header("Game components")]
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private CharacterController _character;
    [SerializeField] private LevelManager _levelManager;

    [Space][Header("Physics")]
    [SerializeField] private float _moveSpeed = 2f;

    private float _gravity;
    private bool _needToMove;

    private void OnEnable()
    {
        _levelManager.OnNewGameStarted += () => _needToMove = true;
        _levelManager.OnGameEnded += () => _needToMove = false;
    }

    private void FixedUpdate()
    {
        if (!_needToMove)
        {
            return;
        }

        _gravity -= 9.81f * Time.deltaTime;
        if (_character.isGrounded)
        {
            _gravity = 0;
        }

        var newPosition = new Vector3(_joystick.Horizontal + Input.GetAxis("Horizontal"), _gravity, _joystick.Vertical + Input.GetAxis("Vertical"));
        _character.Move(newPosition * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Coin>(out var coin))
        {
            coin.OnPlayerGetCoin();
        }
    }
}
