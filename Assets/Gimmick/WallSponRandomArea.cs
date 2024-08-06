using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSponRandomArea : MonoBehaviour
{
    [SerializeField, Header("壁のPrefabを入れてください")] GameObject _WallPrefab;
    GameSystem _GameSystem;
    public int _Time;
    [SerializeField, Header("範囲を決めてください 2まで")] Vector3[] _Pos1;


    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();
        StartCoroutine(Wall());
    }

    // Update is called once per frame
    private IEnumerator Wall()
    {
        while (true)
        {
            yield return new WaitForSeconds(_Time);
            float x = Random.Range(_Pos1[0].x, _Pos1[1].x);
            float y = Random.Range(_Pos1[0].y, _Pos1[1].y);
            Vector3 pos = new Vector3(x, y, 0);
            Instantiate(_WallPrefab, pos, Quaternion.identity);
            Debug.Log($"{pos.ToString()}");
        }

        if (_GameSystem is not null && !_GameSystem.IsPausing)
        {
            while (true)
            {
                yield return new WaitForSeconds(_Time);
                float x = Random.Range(_Pos1[1].x, _Pos1[2].x);
                float y = Random.Range(_Pos1[1].y, _Pos1[2].y);
                Vector3 pos = new Vector3(x, y, 0);
                Instantiate(_WallPrefab, pos, Quaternion.identity);
            }
        }
    }
}

