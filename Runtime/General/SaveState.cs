using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Reflection;
using DollUtil.Attributes;
using DollUtil.Buggy;
using UnityEditor;

namespace DollUtil
{
    [AddComponentMenu("DollUtil/Save State")]
    public class SaveState : MonoBehaviour
    {
        public string File;
        public bool LoadOnAwake = true;
        private Dictionary<string, string> rawData = new Dictionary<string, string>();
        private static SaveState cachedFind;

        private void Awake()
        {
            if (LoadOnAwake) Load();
        }

        /// <summary>
        /// Find reference to SaveState.
        /// </summary>
        /// <returns></returns>
        public static SaveState GrabFirst {
            get
            {
                if(cachedFind == null)
                {
                    cachedFind = GameObject.FindObjectOfType<SaveState>();
                }
                return cachedFind;
            }
        }

        /// <summary>
        /// Save all [SaveData] to file
        /// </summary>
        public void Save()
        {
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in sceneActive)
            {
                if (mono == this) continue;
                var fields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (var f in fields)
                {
                    SaveData attribute = (SaveData)System.Attribute.GetCustomAttribute(f, typeof(SaveData)) as SaveData;
                    if (attribute != null)
                    {
                        UniqueId id = mono.GetComponent<UniqueId>();
                        if (id == null)
                        {
                            Debug.LogError($"No Id for Mono -> {mono.name}");
                            continue;
                        }
                        SaveDataFrom($"{id.uniqueId}.{f.Name}", f.GetValue(mono));
                    }
                }
            }

            SaveToFile();
        }

        /// <summary>
        /// Load all [SaveData] from file
        /// </summary>
        public void Load()
        {
            if (!LoadFromFile()) return;

            SyncBehaviour[] sceneActive = FindObjectsOfType<SyncBehaviour>();
            foreach (SyncBehaviour mono in sceneActive)
            {
                if (mono == this) continue;
                var fields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (var f in fields)
                {
                    SaveData attribute = (SaveData)System.Attribute.GetCustomAttribute(f, typeof(SaveData)) as SaveData;
                    if (attribute != null)
                    {
                        UniqueId id = mono.uId;

                        ISyncable tmp = f.GetValue(mono) as ISyncable;

                        if (tmp != null)
                        {
                            if(rawData.ContainsKey($"{id.uniqueId}.{f.Name}"))
                                tmp.MapFrom(JsonUtility.FromJson(rawData[$"{id.uniqueId}.{f.Name}"], f.FieldType));
                        }
                        else
                        {
                            if (rawData.ContainsKey($"{id.uniqueId}.{f.Name}"))
                                f.SetValue(mono, JsonUtility.FromJson(rawData[$"{id.uniqueId}.{f.Name}"], f.FieldType));
                        }
                    }
                }
            }
        }

        [ContextMenu("Delete Save")]
        public void DeletePersistent()
        {
            if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, File + ".json").ToString()))
            {
                System.IO.File.Delete(Path.Combine(Application.persistentDataPath, File + ".json").ToString());
            }
        }

        private void SaveDataFrom(string slot, object data)
        {
            string serialized = JsonUtility.ToJson(data, true);
            if (rawData == null) rawData = new Dictionary<string, string>();
            if (!rawData.ContainsKey(slot))
            {
                rawData.Add(slot, serialized);
            }
            else
            {
                rawData[slot] = serialized;
            }
        }

        private object ReadDataFrom(string slot)
        {
            if (!rawData.ContainsKey(slot))
            {
                throw new System.Exception("DollUtil Err: Trying to access save data that doesnt exist");
            }
            else
            {
                return JsonConvert.DeserializeObject(rawData[slot]);
            }
        }

        /// <summary>
        /// Save file using currently Saved Data.
        /// </summary>
        /// <remarks>Does not save current object data</remarks>
        public void SaveToFile()
        {
            string jsons = JsonConvert.SerializeObject(rawData);

            using (StreamWriter writetext = new StreamWriter(Path.Combine(Application.persistentDataPath, File + ".json")))
            {
                writetext.WriteLine(jsons);
            }
        }

        /// <summary>
        /// Load file.
        /// </summary>
        /// <remarks>Does not load data into objects.</remarks>
        public bool LoadFromFile()
        {
            string json = "";

            if (!System.IO.File.Exists(Path.Combine(Application.persistentDataPath, File + ".json").ToString()))
            {
                return false;
            }

            using (StreamReader readtext = new StreamReader(Path.Combine(Application.persistentDataPath, File + ".json")))
            {
                json = readtext.ReadLine();
            }

            var runtimeAllGuids = GameObject.FindObjectsOfType<UniqueId>().ToList();

            foreach (var entry in JsonConvert.DeserializeObject<Dictionary<string, string>>(json))
            {
                if(runtimeAllGuids.Find(g => g.uniqueId == entry.Key.Split('.')[0]) != null |
                   entry.Key.Split('_')[0] != gameObject.scene.name)
                {
                    rawData.Add(entry.Key, entry.Value);
                }
            }

            return true;
        }
    }
}