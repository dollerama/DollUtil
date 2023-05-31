using System.Collections;
using System.Collections.Generic;
using DollUtil;
using DollUtil.Buggy.Scribble;
using UnityEngine;

public class SyncSend : SyncBehaviour
{
    public Sphere sphereSend = new Sphere(Vector3.zero, 0.2f, false);

    public override void Update()
    {
        base.Update();

        sphereSend.position = Vector3.right * 2 * Mathf.Sin(Time.time);
    }
}
