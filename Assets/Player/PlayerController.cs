using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField, Header("飛ぶときに加える力")] private float _flyPower = 5f;
    [SerializeField, Header("跳ぶときに加える力")] private float _jumpPower = 0.5f;
    [SerializeField, Header("現在の操作状態")] private PlayerMoveState _currentMoveState = PlayerMoveState.Fly;
    [SerializeField, Header("切り替わる時間の最小時間(秒)")] private float _minChangeTime = 3f;
    [SerializeField, Header("切り替わる時間の最大時間(秒)")] private float _maxChangeTime = 10f;
    [SerializeField, Header("跳ぶときに力を加え続ける高さの限界")] private float _y = 0.5f;      
    
    // ギミックから
    [SerializeField, Header("飛ぶときにアウトな壁のタグ")] private string _flyGOTag = "";
    [SerializeField, Header("跳ぶときにアウトな壁のタグ")] private string _jumpGOTag = "";
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _flyAudioClip;
    [SerializeField] private AudioClip _jumpAudioClip;
    [SerializeField] private AudioClip _clashAudioClip;
    
    private Rigidbody _rb;
    private GameSystem _gameSystem;
    private Animator _animator;
    private bool _isGrounded = false;
    private bool _canJump = false;
    private bool _isInputJump = false;
    private bool _isKeyDown = false;
    private bool _isKeyUp = false;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log(_currentMoveState);
        _gameSystem = GameObject.FindObjectOfType<GameSystem>().GetComponent<GameSystem>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        // ランダムに飛ぶと跳ぶを変更
        //StartCoroutine("ChangePlayerMoveState");
        if (_flyGOTag is null || _jumpGOTag is null)
        {
            Debug.LogWarning("ゲームオーバー用のタグがnullです");
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log($"接地：{_isGrounded}");
        //Debug.Log(_currentMoveState);
        //Debug.Log(_gameSystem.IsPausing);
        if (_gameSystem is null)
        {
            Debug.Log("ゲームシステムがnullです");
        }
        
        PlayerMove();
        // ポーズしていないときは動ける
        if (_gameSystem is not null && !_gameSystem.IsPausing)
        {
            PlayerMove();
        }
    }

    private void Update()
    {
        _isKeyDown = Input.GetKeyDown(KeyCode.Space);
        _isKeyUp = Input.GetKeyUp(KeyCode.Space);
        InputJump();
    }

    private void PlayerMove()
    {
        switch (_currentMoveState)
        {
            case PlayerMoveState.Fly:
                FlyPlayerMove();
                break;
            case PlayerMoveState.Jump:
                JumpPlayerMove();
                break;
        }
    }

    /// <summary>
    /// 飛ぶときの挙動
    /// </summary>
    private void FlyPlayerMove()
    {
        
        if (_isKeyDown)
        {
            //Debug.Log("F");
            _rb.velocity = new Vector3(0, _flyPower, 0); // ToDo:
            _audioSource.PlayOneShot(_flyAudioClip);
        }
    }

    /// <summary>
    /// 跳ぶときの挙動
    /// </summary>
    private void JumpPlayerMove()
    {
        //InputJump();
        if (_isInputJump && transform.position.y < _y)
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }
        
        if (_isGrounded && _isKeyDown)
        {
            _audioSource.PlayOneShot(_jumpAudioClip);
        }
        
        if (_canJump)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse); // ToDo
            _isGrounded = false;
            _canJump = false;
        }
    }
    
    /// <summary>
    /// 長押しと短押しのフラグ
    /// </summary>
    private void InputJump()
    {
        if (_isKeyDown && _isGrounded)
        {
            _isInputJump = true;
        }
        else if (_isKeyUp || transform.position.y > _y)
        {
            _isInputJump = false;
        }
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    private void GameOver()
    {
        _gameSystem.NotifyPlayerIsDeath();
        _audioSource.PlayOneShot(_clashAudioClip);
    }

    /// <summary>
    /// 接地判定とゲームオーバーの接触判定
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if (_currentMoveState == PlayerMoveState.Fly)
        {
            // Flyの時にぶつかったらぶつかったらダメなオブジェクトにぶつかるとゲームオーバー
            if (_flyGOTag is not null && tag == _flyGOTag)
            {
                GameOver();
            }
        }
        else if (_currentMoveState == PlayerMoveState.Jump)
        {
            // Jumpの時にぶつかったらぶつかったらダメなオブジェクトにぶつかるとゲームオーバー
            if (_jumpGOTag is not null && tag == _jumpGOTag)
            {
                GameOver();
            }
            if (tag == "Ground")
            {
                _isGrounded = true;
            }            
        }
    }
    
    /// <summary>
    /// ランダムな時間経過で操作方法を切り替える
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangePlayerMoveState()
    {
        while (true)
        {
            _currentMoveState = PlayerMoveState.Fly;
            Debug.Log("Flyに変更");
            yield return new WaitForSeconds(Random.Range(_minChangeTime, _maxChangeTime));
            Debug.Log("Jumpに変更");
            _currentMoveState = PlayerMoveState.Jump;
            yield return new WaitForSeconds(Random.Range(_minChangeTime, _maxChangeTime));
        }
    }
}
