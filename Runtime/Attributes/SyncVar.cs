using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil.Attributes
{
    /// <summary>
    /// Describes when SyncVar occurs.
    /// </summary>
    [Flags] public enum SyncType
    {
        /// <summary>
        /// Sync each Update.
        /// </summary>
        OnUpdate=0,
        /// <summary>
        /// Sync on Awake.
        /// </summary>
        OnAwake=1,
        /// <summary>
        /// Sync on Start.
        /// </summary>
        OnStart=2
    }

    /// <summary>
    /// Attribute allows variables to be synced from one script to another easily.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SyncVar : Attribute
    {
        public readonly string Id;
        public SyncType Callback;
        public string FromUniqueId;

        /// <summary>
        /// Attribute allows variables to be synced from one script to another easily.
        /// </summary>
        /// <param name="n">
        /// Id to sync,
        /// </param>
        public SyncVar(string n)
        {
            Id = n;
            Callback = SyncType.OnStart;
            FromUniqueId = "";
        }
    }
}
