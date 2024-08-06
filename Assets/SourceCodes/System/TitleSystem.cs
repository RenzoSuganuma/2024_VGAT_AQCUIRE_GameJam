using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] GameObject CreditPanel;
    [SerializeField] AudioClip ButtonDecide;
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
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    public void PlayButtonDecide()
    {
        AudioSource.PlayClipAtPoint(ButtonDecide,transform.position);
    }
}
