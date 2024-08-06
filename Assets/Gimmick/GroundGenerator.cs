using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject _Woll;
    [SerializeField] float _Time = 3f;
    float _Timer = 0;
    GameSystem _GameSystem;
    // Start is called before the first frame update
    void Start()
    {
        _GameSystem = GameObject.FindAnyObjectByType<GameSystem>();

        StartCoroutine(Generate());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private IEnumerator Generate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_Time);
            if (!_GameSystem.IsPausing)
            {
                Instantiate(_Woll, this.transform.position, Quaternion.identity);
                //Debug.Log("床を生成");
            }
        }
    }
}
