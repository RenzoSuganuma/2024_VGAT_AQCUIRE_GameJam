﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] GameObject CreditPanel;
    [SerializeField] AudioSource ButtonDecide;
    [SerializeField] GameObject DDOLSoundPrefab;
    AudioSource _as;
    int _buttonIndex;
    int _delta;
    // Start is called before the first frame update
    void Start()
    {
        CreditPanel.SetActive(false);
        _buttonIndex = 1;

        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_buttonIndex > 0)
            {
                _buttonIndex--;
                _as.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_buttonIndex < 2) 
            {
                _buttonIndex++;
                _as.Play();
            }
        }
    }

    
    public void ActivatePanel()
    {
        CreditPanel.SetActive(true);
    }

    public void DeactivatePanel()
    {
        CreditPanel.SetActive(false);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void PlayButtonDecide()
    {
        ButtonDecide.Play();
    }

    public void PlayDDOLSound()
    {
        Instantiate(DDOLSoundPrefab);
    }
}
