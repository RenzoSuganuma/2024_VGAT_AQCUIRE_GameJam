using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ゲームシステムクラス
/// </summary>
public class GameSystem : MonoBehaviour
{
    /// <summary>
    /// ポーズ入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnPause;
    /// <summary>
    /// リジューム入力が入ったときに呼び出してほしいメソッドはここ
    /// </summary>
    public event Action ToDoOnResume;

    /// <summary>
    /// ジャンプ入力 【トリガー】
    /// </summary>
    public bool IsJumping => Input.GetKeyDown(KeyCode.Space);

    /// <summary>
    /// プレイヤのスピードを返す
    /// </summary>
    public float PlayerSpeed = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
