using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 velocity;
    private Rigidbody body;

    private void OnEnable()
    {
        if(body == null)
        {
            body = GetComponent<Rigidbody>();
        }
        velocity = new Vector3(5, Random.Range(-1.0f, 1.0f), 0);
    }

    private void OnDisable()
    {
        velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
    }
}
