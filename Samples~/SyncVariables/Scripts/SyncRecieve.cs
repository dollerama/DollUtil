using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DollUtil;
using DollUtil.Buggy.Scribble;
using DollUtil.Attributes;

public class SyncRecieve : SyncBehaviour
{
    [SyncVar("sphereSend", Callback = SyncType.OnUpdate, FromUniqueId = "SyncVariablesScene_66975bd3-a23a-494d-8445-2fe9fc3c9ec6")]
    public Sphere sphere = new Sphere(Vector3.zero, 0.2f, false);

    public override void Start()
    {
        base.Start();
        Scribble.Draw += sphere;
    }

    public override void Update()
    {
        base.Update();
    }
}
