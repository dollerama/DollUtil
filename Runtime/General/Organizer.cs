using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DollUtil
{
    /// <summary>
    /// Container to easily hold objects and reference them elsewhere.
    /// </summary>
    [AddComponentMenu("DollUtil/Organizer")]
    public class Organizer : MonoBehaviour
    {
        /// <summary>
        /// GameObjects that container holds.
        /// </summary>
        public string Name;
        public List<GameObject> Data;
        private static Dictionary<string, List<GameObject>> OrganizerCache = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            if (OrganizerCache.ContainsKey(Name))
            {
                Debug.LogError($"Organizer {Name} could not be added because it already exist");
            }
            else
            {
                OrganizerCache.Add(Name, Data);
            }
        }

        /// <summary>
        /// Gets Component from Organizer.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <param name="organizer">Organizer to pull from.</param>
        /// <param name="name">Name of Organizer Data.</param>
        /// <returns>Component from data.</returns>
        public static T Grab<T>(string organizer, string name) where T : Component
        {
            if (OrganizerCache.ContainsKey(organizer))
            {
                var tmp = OrganizerCache[organizer].Find(g => g.name == name);
                return tmp != null ? tmp.GetComponent<T>() : null;
            }
            else
            {
                Debug.LogError($"Organizer {organizer} does not exist");
                return null;
            }
        }

        /// <summary>
        /// Gets Components from Organizer.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <param name="organizer">Organizer to pull from.</param>
        /// <param name="name">Name of Organizer Data.</param>
        /// <returns>Components from data.</returns>
        public static T[] GrabAll<T>(string organizer, string name) where T : Component
        {
            if (OrganizerCache.ContainsKey(organizer))
            {
                var tmp = OrganizerCache[organizer].Find(g => g.name == name);
                return tmp != null ? tmp.GetComponents<T>() : null;
            }
            else
            {
                Debug.LogError($"Organizer {organizer} does not exist");
                return null;
            }
        }

        /// <summary>
        /// Get GameObject from Organizer.
        /// </summary>
        /// <param name="organizer">Organizer to pull from.</param>
        /// <param name="name">Name of of Organizer Data.</param>
        /// <returns>GameObject from data.</returns>
        public static GameObject Grab(string organizer, string name)
        {
            if (OrganizerCache.ContainsKey(organizer))
            {
                return OrganizerCache[organizer].Find(g => g.name == name);
            }
            else
            {
                Debug.LogError($"Organizer {organizer} does not exist");
                return null;
            }
        }
    }
}