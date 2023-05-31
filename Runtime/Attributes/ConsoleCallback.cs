using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil.Attributes
{
    /// <summary>
    /// Mark method as Console Callback.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCallback : Attribute { }
}
