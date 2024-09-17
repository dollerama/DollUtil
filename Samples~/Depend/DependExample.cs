using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DollUtil;
using DollUtil.Attributes;

public class DependExample : DependBehaviour
{
    [Depend(DependType.Root)] public Camera root_cam;
    [Depend(DependType.All)] public SphereCollider all;
    [Depend(DependType.Self)] public Rigidbody self;
    [Depend(DependType.Sibling)] public Rigidbody sibling;
    [Depend(DependType.Child)] public Rigidbody child;
    [Depend(DependType.Ancestor)] public Rigidbody ancestor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
