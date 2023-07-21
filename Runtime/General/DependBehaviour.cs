using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DollUtil.Attributes;
using UnityEngine;

namespace DollUtil
{
    public class DependBehaviour : MonoBehaviour
    {
        public virtual void OnValidate()
        {
            var fieldInfo = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            for (int j = 0; j < fieldInfo.Length; j++)
            {
                Depend attribute = Attribute.GetCustomAttribute(fieldInfo[j], typeof(Depend)) as Depend;

                if (attribute != null)
                {
                    if (attribute._type == DependType.All)
                    {
                        var find = GameObject.FindObjectOfType(fieldInfo[j].FieldType);

                        if(find) fieldInfo[j].SetValue(this, find as object);
                    }
                    else if (attribute._type == DependType.Self)
                    {
                        var find = this.GetComponent(fieldInfo[j].FieldType);

                        if (find) fieldInfo[j].SetValue(this, find as object);
                    }
                    else if (attribute._type == DependType.Sibling)
                    {
                        if (this.transform.parent != null)
                        {
                            var find = this.transform.parent.GetComponentsInChildren(fieldInfo[j].FieldType);
                            foreach (var f in find)
                            {
                                if (f.gameObject != this.gameObject && f.transform.parent == this.transform.parent)
                                {
                                    fieldInfo[j].SetValue(this, f as object);
                                }
                            }
                        }
                    }
                    else if (attribute._type == DependType.Child)
                    {
                        var find = this.GetComponentsInChildren(fieldInfo[j].FieldType);

                        foreach(var f in find)
                            if(f.gameObject != this.gameObject) fieldInfo[j].SetValue(this, f as object);
                    }
                }
            }
        }
    }
}
