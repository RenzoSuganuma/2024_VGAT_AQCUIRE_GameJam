using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, Header("加速度 [m/s^2]")]
    private float _acceleration = 1.0f;

    [SerializeField, Header("速度の最大値 [m/s]")]
    private float _maxVelocity = 10.0f;

    [SerializeField, Header("ゲームオーバーのイベント")]
    private UnityEvent _eventOnGO;

    private static GameSystem _instance;

    public static GameSystem Instance => _instance;

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

    /// <summary>
    /// 速度ベクトルの大きさ
    /// </summary>
    public float PlayerVelocityMagnitude => _velocity;

    private float _elapsedTime = 0f;

    /// <summary>
    /// プレイヤの移動した距離
    /// </summary>
    public static string PlayerScore;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        _instance = null;
        PlayerScore = "";
    }

    private void Awake()
    {
        if (_instance is null)
        {
            _instance = this;
            this.gameObject.name = this.gameObject.name + " 【Saved Instance】 ";
        }

        // DDOL 登録
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        var instanceArray = GameObject.FindObjectsOfType<GameSystem>();
        if (instanceArray.Length > 1)
        {
            foreach (var item in instanceArray)
            {
                if (item != _instance)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        StartCoroutine(nameof(StartCountDown), 3f);

        SetupBackGroundAudio();

        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            _playerStartPoint = player.transform.position;
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
        CheckPauseInput();
        GetPlayerVelocity();

        var speedMeter = GameObject.Find("SpeedMeter");
        if(speedMeter is not null)
        {speedMeter.GetComponent<Text>().text = _velocity.ToString("F2") + " km/h";}
        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            var str = (player.transform.position - _playerStartPoint).magnitude
                .ToString("F5");

            var dis = GameObject.Find("DistanceMeter");
            if(dis is not null)
            {dis.GetComponent<Text>().text = str + " m";}

            PlayerScore = str;
        }
    }

    IEnumerator StartCountDown(int waitingTime)
    {
        for (int i = 0; i < waitingTime; i++)
        {
            yield return new WaitForSeconds(1);
        }

        _isPausing = false;
    }

    private void CheckPauseInput()
    {
        if (Input.GetButtonDown("PauseResume")) // 仮の一時停止キー
        {
            _isPausing = !_isPausing;
            if (_isPausing)
            {
                if (ToDoOnPause is not null)
                {
                    ToDoOnPause(); // pause
                }
            }
            else
            {
                if (ToDoOnResume is not null)
                {
                    ToDoOnResume(); // resume
                }
            }
        }
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
    }

    private void GetPlayerVelocity()
    {
        _velocity = _acceleration * _elapsedTime; // 後でここは直す

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
        _isPausing = true;
        var player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player is not null)
        {
            _playerEndPoint = player.transform.position;
        }
        
        StartCoroutine(GotoResult());
    }

    IEnumerator GotoResult()
    {
        var panel = GameObject.Find("FadingPanel").GetComponent<Animator>();
        if( panel is not null )
        {panel.Play("Fade");}
        yield return new WaitForSeconds(1);
        GameObject.FindAnyObjectByType<SceneLoader>().LoadScene("ResultScene");
    }
}
