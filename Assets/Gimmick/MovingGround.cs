using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    float _velocity = 0f;

    [SerializeField, Header("壁の動く方向 *動かしたい方向に１を入れてください")]
    Vector3 _WallMoveVector = Vector3.zero;

    [SerializeField, Header("壁の動く速度")] float _Speed = 5f;
    GameSystem _gameSystem;
    private Vector3 _destructionPos;

    // Start is called before the first frame update
    void Start()
    {
        //ここから壁のスピードを参照する[
        _gameSystem = GameObject.FindAnyObjectByType<GameSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _velocity = _Speed;

        this.transform.position += new Vector3(_WallMoveVector.x * _Speed * _velocity * Time.deltaTime,
            _WallMoveVector.y * _Speed * _velocity * Time.deltaTime,
            _WallMoveVector.z * _Speed * _velocity * Time.deltaTime);


        if (transform.position.x < _destructionPos.x)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDestructionPos(Vector3 pos)
    {
        _destructionPos = pos;
    }
}
