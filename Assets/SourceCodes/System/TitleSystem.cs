using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] GameObject CreditPanel;
    [SerializeField] Button CreditExit;
    // Start is called before the first frame update
    void Start()
    {
        CreditPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
