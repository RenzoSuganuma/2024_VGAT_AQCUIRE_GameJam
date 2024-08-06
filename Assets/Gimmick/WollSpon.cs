using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WollSpon : MonoBehaviour
{
    [SerializeField] GameObject _Woll;
    [SerializeField] float _Time = 3f;
    float _Timer = 0;
     GameSystem _GameSystem;
    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();
        if (!_GameSystem.IsPausing)
        {
            Instantiate(_Woll, this.transform.position, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_GameSystem.IsPausing)
        {
            _Timer += Time.deltaTime;
            if (_Timer > _Time)
            {
                _Timer = 0;
                Instantiate(_Woll, this.transform.position, Quaternion.identity);
            }
        }
        
    }
}
