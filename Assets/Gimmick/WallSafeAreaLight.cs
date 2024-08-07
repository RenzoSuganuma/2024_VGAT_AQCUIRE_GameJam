using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSafeAreaLight : MonoBehaviour
{
    PlayerController _PlayerState;
    [SerializeField, Header("壁のライトを入れてください")] GameObject _AreaLight;
    Light _Light;
    [SerializeField , Header("壁の光数値")]  float _WallLightIntensity = 5f;
    // Start is called before the first frame update
    void Start()
    {
        _Light = GetComponent<Light>();
        _PlayerState = GameObject.FindAnyObjectByType<PlayerController>();

        //if (_PlayerState.CurrentMoveState == PlayerMoveState.Fly)
        //{
        //    _Light.intensity = 0;
        //}
        //else if (_PlayerState.CurrentMoveState == PlayerMoveState.Jump)
        //{
        //    _Light.intensity = _WallLightIntensity;
        //}
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
            _Light.intensity = _WallLightIntensity;
        }
    }
}
