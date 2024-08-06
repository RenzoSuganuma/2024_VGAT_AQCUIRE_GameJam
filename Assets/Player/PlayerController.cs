using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // GameSystem/isPausing
    //[SerializeField] private GameSystem _gameSystem;
    [SerializeField] private float _movePower = 5f;
    [SerializeField] private float _maxFallSpeed = 15f;

    private Vector3 _oldPos;
    private Vector3 _velocity;
    private Rigidbody _rb;
    private PlayerMoveState _currentMoveState;
    private bool _isInputJumpKey = false;
    private bool _minJumpFlag = false;
    //private bool _isJumping => Input.GetKeyDown(KeyCode.Space);
    private bool _isGrounded = false;

    private bool _canJump = false;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentMoveState = PlayerMoveState.Jump;
        Debug.Log(_currentMoveState);
    }

    void Update()
    {
        Debug.Log(_isGrounded);
        //_velocity = _rb.velocity;
        //if (_gameSystem.IsJumping)
        //{
            switch (_currentMoveState)
            {
                case PlayerMoveState.Fly:
                    FlyPlayerMove();
                    break;
                case PlayerMoveState.Jump:
                    JumpPlayerMove();
                    break;
            }
        //}

        /*if (_isJumping)
        {
            FlyPlayerMove();
        }*/
    }

    /// <summary>
    /// 飛ぶときの挙動
    /// </summary>
    private void FlyPlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("F");
            _rb.velocity = new Vector3(0, _movePower, 0);
        }
    }

    /// <summary>
    /// 跳ぶときの挙動
    /// </summary>
    private void JumpPlayerMove()
    {
        /*if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("J");
            _canJump = true;
        }*/
        /*if (_isGrounded)
        {
            //_canJump = true;
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("J");
                _canJump = true;
            }
            /*Input.GetKeyDown(KeyCode.Space))}
            {
                _canJump = true;
            }

        }*/

        if (transform.position.y < 1f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("J");
                _canJump = true;
            }            
        }


        if (_canJump)
        {
            if (transform.position.y < 1f)
            {
                _rb.AddForce(Vector3.up * _movePower, ForceMode.Impulse);
                _isGrounded = false;
                _canJump = false;
            }
            else
            {
                _canJump = false;
            }
        }


        /*InputJump();
        Vector3 movePower = Vector3.zero;
        movePower += CalcJumpPower(movePower);
        _rb.velocity = movePower * Time.deltaTime;
        _velocity = (transform.position - _oldPos) / Time.deltaTime;
        // 着地中は最小ジャンプフラグをfalseにする
        if (_isGrounded)
        {
            _minJumpFlag = false;
        }*/
    }

    /// <summary>
    /// ジャンプの高さを計算する
    /// </summary>
    /// <param name="movePower"></param>
    /// <returns></returns>
    private Vector3 CalcJumpPower(Vector3 movePower)
    {
        if (_isInputJumpKey && _isGrounded)
        {
            _velocity.y = _movePower;
        }
        else if (!_isInputJumpKey && !_isGrounded && !_minJumpFlag)
        {
            if (_rb.velocity.y <= 0)
            {
                _velocity.y = 0;
            }

            _minJumpFlag = true;
        }

        movePower.y = Mathf.Max(movePower.y, -_maxFallSpeed);

        return movePower;
    }

    private void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isInputJumpKey = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _isInputJumpKey = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_currentMoveState == PlayerMoveState.Jump)
        {
            if (other.gameObject.tag == "Ground")
            {
                _isGrounded = true;
            }            
        }
    }
    
    // Notify
}
