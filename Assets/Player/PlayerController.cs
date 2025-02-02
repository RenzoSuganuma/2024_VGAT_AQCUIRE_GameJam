using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField, Header("飛ぶときに加える力")] private float _flyPower = 5f;
    [SerializeField, Header("跳ぶときに加える力")] private float _jumpPower = 0.5f;
    [SerializeField, Header("現在の操作状態")] private PlayerMoveState _currentMoveState = PlayerMoveState.Fly;

    [SerializeField, Header("切り替わる時間の最小時間(秒)")]
    private float _minChangeTime = 3f;

    [SerializeField, Header("切り替わる時間の最大時間(秒)")]
    private float _maxChangeTime = 10f;

    [SerializeField, Header("跳ぶときに力を加え続ける高さの限界")]
    private float _y = 0.5f;

    [SerializeField, Header("切り替え予兆の時間")]　private float _time = 2f; //ToDO:命名考える

    [SerializeField, Header(" 落下中の重力の減衰係数。大きければ減衰量が大きくなる ")]
    private float _gravityDecay;

    //[SerializeField, Header(" 落下中の重力の減衰係数。大きければ減衰量が大きくなる(跳ぶとき)")]
    //private float _jumpGravityDecay = 5f;

    // レベルデザイン用
    [SerializeField, Header("飛ぶときの重力")]
    private float _flyGravity = -9.81f;

    [SerializeField, Header("跳ぶときの重力")]
    private float _jumpGravity = -9.81f;

    // ギミックから
    [SerializeField, Header("飛ぶときにアウトな壁のタグ")]
    private string _flyGOTag = "";

    [SerializeField, Header("跳ぶときにアウトな壁のタグ")]
    private string _jumpGOTag = "";

    [SerializeField, Header("地面のタグ")] private string _groundTag = "Ground";

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _flyAudioClip;
    [SerializeField] private AudioClip _jumpAudioClip;
    [SerializeField] private AudioClip _clashAudioClip;
    [SerializeField] private AudioClip _changeAudioClip;

    private Rigidbody _rb;
    private GameSystem _gameSystem;
    private Animator _animator;
    private Renderer _renderer;
    private bool _isGrounded = false;
    private bool _canJump = false;
    private bool _isInputJump = false;
    private bool _isKeyDown = false;
    private bool _isKeyUp = false;
    private bool _isKey = false;

    public PlayerMoveState CurrentMoveState => _currentMoveState;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log(CurrentMoveState);
        _gameSystem = GameObject.FindObjectOfType<GameSystem>().GetComponent<GameSystem>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<Renderer>();
        // ランダムに飛ぶと跳ぶを変更
        StartCoroutine("ChangePlayerMoveState");
        if (_flyGOTag is null || _jumpGOTag is null)
        {
            Debug.LogWarning("ゲームオーバー用のタグがnullです");
        }
    }

    private void FixedUpdate()
    {
        ColorChange();
        //Debug.Log($"接地：{_isGrounded}");
        //Debug.Log(_currentMoveState);
        //Debug.Log(_gameSystem.IsPausing);
        if (_gameSystem is null)
        {
            Debug.Log("ゲームシステムがnullです");
        }

        //SeitchPlayerMovement();
        // ポーズしていない時動ける
        if (_gameSystem is not null && !_gameSystem.IsPausing)
        {
            //SeitchPlayerMovement();
            // 跳ぶ
            // Updateに"飛ぶ"の処理
            if (_currentMoveState == PlayerMoveState.Jump)
            {
                Physics.gravity = new Vector3(0, _jumpGravity, 0);
                JumpPlayerMove();
            }
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezePositionX;
            _rb.constraints = RigidbodyConstraints.FreezePositionZ;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        GravityDecay();
    }

    private void GravityDecay()
    {
        if (_rb.velocity.y < 0)
        {
            var v = _rb.velocity;
            if (v.y < 0f)
            {
                v.y += -(v.y / _gravityDecay);
            }

            _rb.velocity = v;
        }
    }

    private void Update()
    {
        if (!_gameSystem.IsPausing)
        {
            _isKeyDown = Input.GetButtonDown("Jump");
            _isKeyUp = Input.GetButtonUp("Jump");
            _isKey = Input.GetButton("Jump");
            InputJump();
        }
        
        if (_gameSystem is not null && !_gameSystem.IsPausing)
        {
            // 飛ぶ
            // FixedUpdateに"跳ぶ"の処理
            if (_currentMoveState == PlayerMoveState.Fly)
            {
                Physics.gravity = new Vector3(0, _flyGravity, 0);
                FlyPlayerMove();
            }
        }
    }

    
    private void SeitchPlayerMovement()
    {
        switch (CurrentMoveState)
        {
            case PlayerMoveState.Fly:
                Physics.gravity = new Vector3(0, _flyGravity, 0);
                FlyPlayerMove();
                break;
            case PlayerMoveState.Jump:
                Physics.gravity = new Vector3(0, _jumpGravity, 0);
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
            Debug.Log("F");
            _rb.velocity = new Vector3(0, _flyPower, 0);
            _audioSource.PlayOneShot(_flyAudioClip);
        }
    }

    /// <summary>
    /// 跳ぶときの挙動
    /// </summary>
    private void JumpPlayerMove()
    {
        // 力を加える高さに制限を付ける
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
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
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
    /// 状態に合わせて色を変える
    /// </summary>
    private void ColorChange()
    {
        if (_currentMoveState == PlayerMoveState.Fly)
        {
            Color c = new Color(1, .5f, 1, 1);
            _renderer.material.color = c;
        }
        else if (_currentMoveState == PlayerMoveState.Jump)
        {
            _renderer.material.color = Color.yellow;
        }
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if (CurrentMoveState == PlayerMoveState.Fly && tag == _groundTag)
        {
            GameOver();
        }
        else if (CurrentMoveState == PlayerMoveState.Jump)
        {
            if (tag == _groundTag)
            {
                _isGrounded = true;
            }
        }
    }

    /// <summary>
    /// ゲームオーバー判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (CurrentMoveState == PlayerMoveState.Fly)
        {
            // Flyの時にぶつかったらぶつかったらダメなオブジェクトにぶつかるとゲームオーバー
            if (_flyGOTag is not null && tag == _flyGOTag)
            {
                GameOver();
            }
        }
        else if (CurrentMoveState == PlayerMoveState.Jump)
        {
            // Jumpの時にぶつかったらぶつかったらダメなオブジェクトにぶつかるとゲームオーバー
            if (_jumpGOTag is not null && tag == _jumpGOTag)
            {
                GameOver();
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
            yield return new WaitUntil(() => _gameSystem.IsPausing == false);   // ポーズ解除でコルーチン再開
            
            _animator.SetBool("ChangeBefore", false);
            _audioSource.Stop();
            _currentMoveState = PlayerMoveState.Fly;
            _gameSystem.OnPlayerStateChanges();
            
            Debug.Log("Flyに変更");
            yield return new WaitForSeconds(Random.Range(_minChangeTime, _maxChangeTime - _time));
            _animator.SetBool("ChangeBefore", true);
            _audioSource.PlayOneShot(_changeAudioClip);
            Debug.Log($"{_time}秒後に状態をJumpに切り替えます");
            yield return new WaitForSeconds(_time);

            _animator.SetBool("ChangeBefore", false);
            _audioSource.Stop();
            _currentMoveState = PlayerMoveState.Jump;
            _gameSystem.OnPlayerStateChanges();
            Debug.Log("Jumpに変更");
            yield return new WaitForSeconds(Random.Range(_minChangeTime, _maxChangeTime - _time));

            _animator.SetBool("ChangeBefore", true);
            _audioSource.PlayOneShot(_changeAudioClip);
            Debug.Log($"{_time}秒後に状態をFlyに切り替えます");
            yield return new WaitForSeconds(_time);
        }
    }
}
