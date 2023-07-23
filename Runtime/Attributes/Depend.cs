using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil.Attributes
{
    public enum DependType
    {
        All,
        Root,
        Self,
        Sibling,
        Child,
        Ancestor
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class Depend : Attribute
    {
        public DependType _type;
        public Depend(DependType _t)
        {
            _type = _t;
        }
    }
}
