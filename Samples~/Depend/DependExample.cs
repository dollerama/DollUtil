using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DollUtil;
using DollUtil.Attributes;

public class DependExample : DependBehaviour
{
    [Depend(DependType.All)] public Camera main_cam;
    [Depend(DependType.Self)] public Rigidbody self;
    [Depend(DependType.Sibling)] public Rigidbody sibling;
    [Depend(DependType.Child)] public Rigidbody child;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
