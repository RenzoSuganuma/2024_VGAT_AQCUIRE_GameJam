using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// すべてのシーンに個々のインスタンスが存在する
/// </summary>
public class GameSystemAccessor : MonoBehaviour
{
    private GameSystem _gameSystem;
    
    private void Start()
    {
        _gameSystem = GameSystem.Instance;
    }

    /// <summary>
    /// システムのシーン読み込みメソッドを呼び出す
    /// </summary>
    public void CallLoadNextScene()
    {
        _gameSystem.LoadNextScene();
    }
}
