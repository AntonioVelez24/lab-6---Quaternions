using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float speed = 20f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Vector3 velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, -speed);
        _rigidbody.velocity = velocity;

        if (transform.position.z <= -10)
        {
            Destroy(gameObject); 
        }
    }
}
