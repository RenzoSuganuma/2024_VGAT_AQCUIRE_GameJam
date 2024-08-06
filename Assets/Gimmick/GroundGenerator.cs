using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject _Wall;
    [SerializeField] float _Time = 3f;
    [SerializeField] private Vector3 _destructionBorder;
    GameSystem _GameSystem;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _destructionBorder, 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();

        StartCoroutine(Generate());
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private IEnumerator Generate()
    {
        while (true)
        {
            var obj = Instantiate(_Wall, this.transform.position, Quaternion.identity);
            var c = obj.GetComponent<MovingGround>();
            c.SetDestructionPos(transform.position + _destructionBorder);
            yield return new WaitForSeconds(_Time);
        }
    }
}
