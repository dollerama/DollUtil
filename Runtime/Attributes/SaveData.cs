using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil.Attributes
{
    /// <summary>
    /// Attribute marks class as save data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SaveData : Attribute
    {
    }
}
