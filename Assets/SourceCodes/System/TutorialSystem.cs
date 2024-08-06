using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] UnityEvent PressSpace;
    [SerializeField] GameObject SoundPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            PressSpace.Invoke();
        }
    }

    public void PlaySound()
    {
        Instantiate(SoundPrefab);
    }
}
