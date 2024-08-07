using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaLighting : MonoBehaviour
{
    PlayerController _PlayerState;
    [SerializeField, Header("壁のライトを入れてください")] GameObject _AreaLight;
    Light _Light;
    [SerializeField, Header("床の光数値")] float _GroundLightIntensity = 400f;
    // Start is called before the first frame update
    void Start()
    {
        _Light =   GetComponent<Light>();
        _PlayerState = GameObject.FindAnyObjectByType<PlayerController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_PlayerState.CurrentMoveState == PlayerMoveState.Fly)
        {
            _Light.intensity = 0;
        }
        else if (_PlayerState.CurrentMoveState == PlayerMoveState.Jump)
        {
            _Light.intensity = _GroundLightIntensity;
        }
    }
}
