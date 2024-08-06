using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLSound : MonoBehaviour
{
    AudioSource _as;
    bool isEnough = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _as = GetComponent<AudioSource>();
        StartCoroutine(Timer() );
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnough == true)
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator Timer()
    {
        isEnough = false;
        float timer = 0;
        while (isEnough == false)
        {
            yield return null;
            timer += Time.deltaTime;
            if (timer > 2) 
            { 
                isEnough = true;
            }
        }
    }
}
