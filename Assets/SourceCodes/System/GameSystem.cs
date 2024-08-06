using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  ゲームシステムクラス
/// </summary>
public sealed class GameSystem : MonoBehaviour
{
    [SerializeField, Header("加速度 [m/s^2]")]
    private float _acceleration = 1.0f;

    [SerializeField, Header("速度の最大値 [m/s]")]
    private float _maxVelocity = 10.0f;

    /// <summary>
    /// ポーズ入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnPause;

    /// <summary>
    /// リジューム入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnResume;

    private bool _isPausing = false;

    public bool IsPausing => _isPausing;

    private bool _isJumping = false;

    /// <summary>
    /// ジャンプ入力 【トリガー】
    /// </summary>
    public bool IsJumping => _isJumping;

    private float _velocity = 1.0f;

    /// <summary>
    /// 速度ベクトルの大きさ
    /// </summary>
    public float PlayerVelocityMagnitude => _velocity;

    private float _elapsedTime = 0f;

    private void Awake()
    {
        // DDOL 登録
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
    }

    private void Update()
    {
        CheckPauseInput();
        GetPlayerVelocity();
        _isJumping = Input.GetKeyDown(KeyCode.Space);
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // 仮の一時停止キー
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
    /// 次のシーンを読み込む
    /// </summary>
    public void LoadNextScene()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneName = scene.name;

        switch (sceneName)
        {
            case "TitleScene":
                SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
                break;
            case "TutorialScene":
                SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
                break;
            case "MainScene":
                SceneManager.LoadScene("ResultScene", LoadSceneMode.Single);
                break;
            case "ResultScene":
                SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
                break;
        }
    }
}
