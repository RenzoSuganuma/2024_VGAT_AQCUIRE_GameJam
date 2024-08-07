using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallPairGenerator : MonoBehaviour
{
    [SerializeField, Header("壁のPrefabを入れてください")]
    GameObject _WallPrefab;

    GameSystem _GameSystem;
    public int _Time;

    [SerializeField, Header("範囲を決めてください 2まで")]
    Vector3[] _Pos1;

    [SerializeField] private Vector3 _destructionBorder;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _destructionBorder, 1);
    }

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
            float x = transform.position.x;
            float y = Random.Range(_Pos1[0].y, _Pos1[1].y);
            Vector3 pos = new Vector3(x, y, 0);
            if (!_GameSystem.IsPausing)
            {
                var obj = Instantiate(_WallPrefab, pos, Quaternion.identity);
                var c = obj.GetComponent<MovingWall>();
                c.SetDestructionPos(transform.position + _destructionBorder);
                yield return new WaitForSeconds(_Time - ( _GameSystem.PlayerVelocityMagnitude * .1f ));
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
