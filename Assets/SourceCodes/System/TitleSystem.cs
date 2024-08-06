using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] GameObject CreditPanel;
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

    public void OuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}
