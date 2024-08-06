using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    float _playerVelocity = 0f;
    [SerializeField, Header("壁の動く方向 *動かしたい方向に１を入れてください")] Vector3 _WallMoveVector = Vector3.zero;
    [SerializeField, Header("壁の動く速度")] float _Speed = 5f;
    [SerializeField, Header("壁の消える距離")] float _WallDellte = 20f;
     GameSystem _gameSystem;

    // Start is called before the first frame update
    void Start()
    {

        //ここから壁のスピードを参照する[
        _gameSystem = GameObject.FindAnyObjectByType<GameSystem>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_gameSystem.IsPausing)
        {
            _playerVelocity = _gameSystem.PlayerVelocityMagnitude;
            this.transform.position += new Vector3(_WallMoveVector.x * _Speed  *  _playerVelocity * Time.deltaTime, _WallMoveVector.y * _Speed * _playerVelocity * Time.deltaTime, _WallMoveVector.z * _Speed * _playerVelocity * Time.deltaTime);
            if (this.transform.position.x <= _WallDellte)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
