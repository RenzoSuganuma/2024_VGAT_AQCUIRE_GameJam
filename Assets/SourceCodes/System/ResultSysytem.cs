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
        //ScoreText.text = GameSystem.PlayerScore;
        ScoreText.color = new Color(1f, 1f, 1f, 0f);
        ScoreText.CrossFadeAlpha(1, 0.1f, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        Instantiate(SoundPrefab);
    }
}
