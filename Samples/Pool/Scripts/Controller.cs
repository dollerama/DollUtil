using System.Collections;
using System.Collections.Generic;
using DollUtil;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pool object_pool;
    public Pool bullet_pool;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        object_pool.Init(this);
        bullet_pool.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            object_pool.Grab(Random.insideUnitSphere * 2, Quaternion.identity, 3f);
            timer = 0;
        }

        if(Input.anyKey)
        {
            bullet_pool.Grab(new Vector3(-5, Random.Range(-1.0f, 1.0f), 0), Quaternion.identity, g => g.transform.position.x > 5.0f);
        }
    }
}
