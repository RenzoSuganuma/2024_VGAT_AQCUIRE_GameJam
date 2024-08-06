using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    // GameSystem/isPausing
    [SerializeField] private float _flyPower = 5f;
    [SerializeField] private float _jumpPower = 0.5f;
    [SerializeField] private float _maxFallSpeed = 15f;
    [SerializeField] private PlayerMoveState _currentMoveState = PlayerMoveState.Fly;
    
    private Rigidbody _rb;
    private bool _isGrounded = false;
    private bool _canJump = false;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log(_currentMoveState);
    }

    void Update()
    {
        Debug.Log(_isGrounded);

        switch (_currentMoveState)
        {
            case PlayerMoveState.Fly:
                FlyPlayerMove();
                break;
            case PlayerMoveState.Jump:
                JumpPlayerMove();
                break;
        }
    }

    /// <summary>
    /// 飛ぶときの挙動
    /// </summary>
    private void FlyPlayerMove()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("F");
            _rb.velocity = new Vector3(0, _flyPower, 0);
        }
    }

    /// <summary>
    /// 跳ぶときの挙動
    /// </summary>
    private void JumpPlayerMove()
    {
        if (transform.position.y < 1f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("J");
                _canJump = true;
            }            
        }
        
        if (_canJump)
        {
            if (transform.position.y < 1f)
            {
                _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
                _isGrounded = false;
                _canJump = false;
            }
            else
            {
                _canJump = false;
            }
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
