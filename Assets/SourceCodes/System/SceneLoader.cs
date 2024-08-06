using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// すべてのシーンに個々のインスタンスが存在する
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image Panel;
    private void Start()
    {
        if(Panel != null)
        {
            StartCoroutine(FadeIn());
        }
        
    }
    /// <summary>
    /// システムのシーン読み込みメソッドを呼び出す
    /// </summary>
    public void LoadScene(string name)
    {
        StartCoroutine(Fadeout(name));
    }

    IEnumerator FadeIn()
    {
        Panel.CrossFadeAlpha(0, 0.3f, false);
        yield return new WaitForSeconds(0.3f);
        Panel.gameObject.SetActive(false);
    }
    IEnumerator Fadeout(string name)
    {
        if (Panel != null)
        {
            Panel.gameObject.SetActive(true);
            Panel.CrossFadeAlpha(1, 0.3f, false);
            yield return new WaitForSeconds(0.3f); 
        }
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
