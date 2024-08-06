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
    /// <summary>
    /// システムのシーン読み込みメソッドを呼び出す
    /// </summary>
    public void CallLoadNextScene()
    {
        GameObject.FindAnyObjectByType<GameSystem>().LoadNextScene();
    }
}
