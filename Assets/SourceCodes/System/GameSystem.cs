using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///  ゲームシステムクラス
/// </summary>
public sealed class GameSystem : MonoBehaviour
{
    [SerializeField, Header("開始時の速度")] private float _startVelo = 1.0f;

    [SerializeField, Header("加速度 [m/s^2]")]
    private float _acceleration = 1.0f;

    [SerializeField, Header("速度の最大値 [m/s]")]
    private float _maxVelocity = 10.0f;

    [SerializeField, Header("ゲームオーバーのイベント")]
    private UnityEvent _eventOnGO;

    [SerializeField] private GameObject _playerDeathParticle;

    public float MaxVelocity => _maxVelocity;

    /// <summary>
    /// ポーズ入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnPause;

    /// <summary>
    /// リジューム入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnResume;

    private bool _isPausing = true;

    public bool IsPausing => _isPausing;

    private float _velocity = 1.0f;

    private Vector3 _playerStartPoint;
    private Vector3 _playerEndPoint;

    private Sprite _imgJump, _imgFly;

    /// <summary>
    /// 速度ベクトルの大きさ
    /// </summary>
    public float PlayerVelocityMagnitude => _velocity;

    private float _elapsedTime = 0f;

    /// <summary>
    /// プレイヤの移動した距離
    /// </summary>
    public static string PlayerScore;

    /// <summary>
    /// スコア加算用の変数
    /// </summary>
    private float _score = 0f;

    private bool _isGameOver = false;


    private void Start()
    {
        StartCoroutine(WaitPressSpaceKey());

        SetupBackGroundAudio();

        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            _playerStartPoint = player.transform.position;
        }
        
        _imgFly = Resources.Load<Sprite>("Images/icon_fly");
        _imgJump = Resources.Load<Sprite>("Images/icon_jump");
    }

    public void OnPlayerStateChanges()
    {
        var img = GameObject.Find("PlayerStateImage").GetComponent<Image>();
        var state = GameObject.FindAnyObjectByType<PlayerController>().CurrentMoveState;
        switch (state)
        {
            case PlayerMoveState.Fly:
                img.sprite = _imgFly;
                break;
            case PlayerMoveState.Jump:
                img.sprite = _imgJump;
                break;
        }
    }

    private void SetupBackGroundAudio()
    {
        gameObject.AddComponent<AudioSource>();
        var audioSources = GetComponent<AudioSource>();
        audioSources.loop = true;
        audioSources.clip = Resources.Load<AudioClip>("Sounds/BGM");
        audioSources.Play();
    }

    private void Update()
    {
        if (_isPausing) return;


        GetPlayerVelocity();

        var speedMeter = GameObject.Find("SpeedMeter");
        if (speedMeter is not null)
        {
            speedMeter.GetComponent<Text>().text = _velocity.ToString("F2") + " \nkm/h";
        }

        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            _score += _velocity * Time.deltaTime;
            var str = _score.ToString("F2");

            var dis = GameObject.Find("DistanceMeter");
            if (dis is not null)
            {
                dis.GetComponent<Text>().text = str + " m";
            }

            PlayerScore = str;
        }
    }

    IEnumerator WaitPressSpaceKey()
    {
        var text = GameObject.Find("GameStateText");
        if (text is not null)
        {
            text.GetComponent<Text>().text = "Press SPACE to Start";
        }

        // Spaceが押されるまで待機する
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (text is not null)
        {
            text.GetComponent<Text>().text = "";
        }

        _isPausing = false;
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
    }

    private void OnDisable()
    {
    }

    private void GetPlayerVelocity()
    {
        _velocity = _startVelo + _acceleration * _elapsedTime; // 後でここは直す

        if (_maxVelocity < _velocity)
        {
            _velocity = _maxVelocity;
        } // クランプ処理
    }

    /// <summary>
    /// DDOl登録の解除とオブジェクトの破棄の実行をする
    /// </summary>
    public void DisposeThisObject()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(this.gameObject, scene);
        GameObject.Destroy(this.gameObject);
    }

    /// <summary>
    /// 移動した距離を渡されるベクトルで計算して小数点以下５ｹﾀでTextへ表示する
    /// </summary>
    public void DisplayMovedDistance(Vector3 start, Vector3 end, Text text)
    {
        var d = (end - start).magnitude;
        text.text = d.ToString("F5");
    }

    /// <summary>
    /// 次のシーンを読み込む
    /// </summary>
    public void LoadNextScene()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneName = scene.name;
        var loader = GameObject.FindAnyObjectByType<SceneLoader>();

        if (sceneName is "MainScene")
        {
            PlayerScore = "";
        }

        switch (sceneName)
        {
            case "TitleScene":
                loader.LoadScene("TutorialScene");
                break;
            case "TutorialScene":
                loader.LoadScene("MainScene");
                break;
            case "MainScene":
                loader.LoadScene("ResultScene");
                break;
            case "ResultScene":
                loader.LoadScene("TitleScene");
                break;
        }
    }

    /// <summary>
    /// プレイヤ死亡時にこれを呼び出す
    /// </summary>
    public void NotifyPlayerIsDeath()
    {
        if (_isGameOver)
        {
            return;
        }

        _isGameOver = true;
        _isPausing = true;

        var text = GameObject.Find("GameStateText");
        if (text is not null)
        {
            text.GetComponent<Text>().text = "GAME OVER !!";
        }

        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            _playerEndPoint = player.transform.position;
            var part = GameObject.Instantiate(_playerDeathParticle, player.transform.position,
                player.transform.rotation, player.transform);
        }

        var cam = Camera.main;
        cam.transform.transform.DOPunchPosition(Vector3.one - Vector3.forward, .5f);

        StartCoroutine(GotoResult());
    }

    IEnumerator GotoResult()
    {
        _eventOnGO.Invoke();
        yield return new WaitForSeconds(1);
        GameObject.FindAnyObjectByType<SceneLoader>().LoadScene("ResultScene");
    }
}
