using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DollUtil.Buggy.Scribble
{
    /// <summary>
    /// Scribble makes gizmo and editor objects easier to draw and keep persistent if necessary.
    /// </summary>
    public class Scribble : MonoBehaviour
    {
        static Scribble mInstance;

        /// <summary>
        /// static instance for Scribble.
        /// </summary>
        public static Scribble Draw
        {
            get
            {
                if (mInstance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Scribble";
                    mInstance = go.AddComponent<Scribble>();
                }
                return mInstance;
            }

            set
            {
                if (mInstance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Scribble";
                    mInstance = go.AddComponent<Scribble>();
                }
                mInstance = value;
            }
        }

        private Stack<IScribbable> ImmediateToDraw = new Stack<IScribbable>();
        private List<IScribbable> ToDraw = new List<IScribbable>();

        /// <summary>
        /// Add IScribbable Object to Immediate Stack.
        /// </summary>
        /// <param name="b">Scribble</param>
        /// <param name="c">IScribbable to Add.</param>
        /// <returns>Scribble</returns>
        public static Scribble operator *(Scribble b, IScribbable c)
        {
            b.Immediate(c);
            return b;
        }

        /// <summary>
        /// Add IScribbable Object to Persistent Stack.
        /// </summary>
        /// <param name="b">Scribble</param>
        /// <param name="c">IScribbable to Add.</param>
        /// <returns>Scribble</returns>
        public static Scribble operator +(Scribble b, IScribbable c)
        {
            b.AddPersistent(c);
            return b;
        }

        /// <summary>
        /// Remove IScribbable Object from Persistent Stack.
        /// </summary>
        /// <param name="b">Scribble</param>
        /// <param name="c">IScribbable to Remove.</param>
        /// <returns>Scribble</returns>
        public static Scribble operator -(Scribble b, IScribbable c)
        {
            b.RemovePersistent(c);
            return b;
        }

        public void Immediate(IScribbable c) => ImmediateToDraw.Push(c);
        public void AddPersistent(IScribbable c) => ToDraw.Add(c);
        public void RemovePersistent(IScribbable c) => ToDraw.Remove(c);

        private void OnDrawGizmos()
        {
            while (ImmediateToDraw.Count > 0)
            {
                var entry = ImmediateToDraw.Pop();
                entry.Draw();
            }

            foreach (var entry in ToDraw)
            {
                entry.Draw();
            }
        }
    }
}
