using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPairInstantiator : MonoBehaviour
{
    GameSystem _GameSystem;
    [SerializeField, Header("壁をランダムで選択して生成する")] GameObject[] _WallSpon;
    [SerializeField, Header("壁の生成場所")] Vector3 _SponPosition;
    int _Random;
    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Instantiate(_WallSpon[Random.Range(1, _WallSpon.Length)], new Vector3(_SponPosition.x, _SponPosition.y, _SponPosition.z), Quaternion.identity);
        if (_GameSystem is not null && !_GameSystem.IsPausing)
        {
            Instantiate(_WallSpon[Random.Range(1, _WallSpon.Length)], new Vector3(_SponPosition.x, _SponPosition.y, _SponPosition.z), Quaternion.identity);
        }

    }
}
