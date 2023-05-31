using System.Collections;
using System.Collections.Generic;
using DollUtil;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pool object_pool;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        object_pool.Init(this);
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
    }
}
