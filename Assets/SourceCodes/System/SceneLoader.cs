using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// すべてのシーンに個々のインスタンスが存在する
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// システムのシーン読み込みメソッドを呼び出す
    /// </summary>
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
