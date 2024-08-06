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
        if (_GameSystem is not null && !_GameSystem.IsPausing)
        {
            GameObject childObject = Instantiate(_WallSpon[Random.Range(0, _WallSpon.Length)], this.transform.position);
            childObject.transform.localPosition = new Vector3(0, 2, 0);
            childObject.transform.localRotation = Quaternion.identity;
            childObject.transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
