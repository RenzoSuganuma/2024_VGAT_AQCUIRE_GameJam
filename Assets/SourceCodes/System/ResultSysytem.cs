using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSysytem : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject SoundPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //ScoreText.color = new Color(1, 1, 1, 0);
        ScoreText.text = GameSystem.PlayerScore;
        //StartCoroutine(TextFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        Instantiate(SoundPrefab);
    }

    IEnumerator TextFade()
    {
        yield return new WaitForSeconds(0.3f);
        ScoreText.CrossFadeAlpha(1, 0.1f, false);
    }
}
