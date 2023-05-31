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
        public List<GameObject> Data;
        private static List<GameObject> OrganizerCache = new List<GameObject>();

        private void Awake()
        {
            OrganizerCache.AddRange(Data);
        }

        /// <summary>
        /// Gets Component from Organizer.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <param name="name">Name of Organizer Data.</param>
        /// <returns>Component from data.</returns>
        public static T Grab<T>(string name) where T : Component
        {
            var tmp = OrganizerCache.Find(g => g.name == name);
            return tmp != null ? tmp.GetComponent<T>() : null;
        }

        /// <summary>
        /// Gets Components from Organizer.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <param name="name">Name of Organizer Data.</param>
        /// <returns>Components from data.</returns>
        public static T[] GrabAll<T>(string name) where T : Component
        {
            var tmp = OrganizerCache.Find(g => g.name == name);
            return tmp != null ? tmp.GetComponents<T>() : null;
        }

        /// <summary>
        /// Get GameObject from Organizer.
        /// </summary>
        /// <param name="name">Name of of Organizer Data.</param>
        /// <returns>GameObject from data.</returns>
        public static GameObject Grab(string name) => OrganizerCache.Find(g => g.name == name);
    }
}