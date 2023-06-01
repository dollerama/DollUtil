using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using DollUtil.Attributes;

namespace DollUtil
{
    /// <summary>
    /// MonoSyncBehaviour to be used in conjunction with Attribute [SyncVar].
    /// </summary>
    [RequireComponent(typeof(UniqueId))]
    public class SyncBehaviour : MonoBehaviour
    {
        private List<FieldInfo> thisFields = null;
        private Dictionary<SyncBehaviour, FieldInfo[]> cachedFields = new Dictionary<SyncBehaviour, FieldInfo[]>();
        private static List<SyncBehaviour> cachedSyncBehaviours = new List<SyncBehaviour>();
        private UniqueId unique_id;
        public UniqueId uId
        {
            get
            {
                if(unique_id == null)
                {
                    unique_id = GetComponent<UniqueId>();
                }
                return unique_id;
            }
        }

        private void Sync(SyncType st)
        {
            if (thisFields == null)
            {
                thisFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).ToList();
            }

            foreach (SyncBehaviour mono in cachedSyncBehaviours)
            {
                if (!cachedFields.ContainsKey(mono))
                {
                    cachedFields.Add(mono, mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public));
                }

                for (int i = 0; i < cachedFields[mono].Length; i++)
                {
                    for (int j = 0; j < thisFields.Count; j++)
                    {
                        SyncVar attribute = Attribute.GetCustomAttribute(thisFields[j], typeof(SyncVar)) as SyncVar;

                        if (attribute != null && st.HasFlag(attribute.Callback))
                        {
                            if (attribute.FromUniqueId != "")
                            {
                                if (mono.uId.uniqueId != attribute.FromUniqueId) break;
                            }

                            if (cachedFields[mono][i].Name == attribute.Id)
                            {
                                ISyncable tmp = thisFields[j].GetValue(this) as ISyncable;
                                if (tmp != null)
                                {
                                    tmp.MapFrom(cachedFields[mono][i].GetValue(mono));
                                }
                                else
                                { 
                                    thisFields[j].SetValue(this, cachedFields[mono][i].GetValue(mono));
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual void Awake()
        {
            cachedSyncBehaviours.Add(this);

            Sync(SyncType.OnAwake | SyncType.OnUpdate);
        }

        public virtual void Start()
        {
            Sync(SyncType.OnStart | SyncType.OnUpdate);
        }

        public virtual void Update()
        {
            Sync(SyncType.OnUpdate);
        }
    }
}
