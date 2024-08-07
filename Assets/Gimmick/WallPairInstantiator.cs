using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPairInstantiator : MonoBehaviour
{
    GameSystem _GameSystem;
    [SerializeField, Header("壁をランダムで選択して生成する上壁")] GameObject[] _UpperWall;
    [SerializeField, Header("壁をランダムで選択して生成する下壁")] GameObject[] _UnderWall;
    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();
        if (_GameSystem is not null && !_GameSystem.IsPausing)
        {
            int UpperWallRandom = Random.Range(0, _UpperWall.Length - 1);
            int UnderWallRandom = Random.Range(0, _UnderWall.Length - 1);  
            _UpperWall[UpperWallRandom].SetActive(true);
            _UnderWall[UnderWallRandom].SetActive(true);
            Debug.Log("1");
        }
    }
}
