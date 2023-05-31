using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Remap a value from one range to another.
        /// </summary>
        /// <param name="value">Value to remap.</param>
        /// <param name="from1">Range A start.</param>
        /// <param name="to1">Range A end.</param>
        /// <param name="from2">Range B start.</param>
        /// <param name="to2">Range B end.</param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
