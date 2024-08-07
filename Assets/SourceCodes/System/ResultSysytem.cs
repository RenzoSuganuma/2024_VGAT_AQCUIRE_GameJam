﻿using System.Collections;
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
        ScoreText.text = GameSystem.PlayerScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
