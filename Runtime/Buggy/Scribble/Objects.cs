using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DollUtil.Buggy.Scribble
{
    public interface IScribbable
    {
        public void Draw();
    }

    [Serializable]
    public class Line : IEquatable<Line>, IScribbable, ISyncable
    {
        public Vector3 start;
        public Vector3 end;
        public bool arrow;

        public Line(Vector3 _start, Vector3 _end, bool with_arrow = false)
        {
            start = _start;
            end = _end;
            arrow = with_arrow;
        }

        public static bool operator ==(Line obj1, Line obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Line obj1, Line obj2) => !(obj1 == obj2);

        public bool Equals(Line other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (start == other.start && end == other.end);
        }
        
        public override bool Equals(object obj) => Equals(obj as Line);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = start.GetHashCode();
                hashCode = (hashCode * 397) ^ end.GetHashCode();
                return hashCode;
            }
        }

        public void Draw()
        {
            if (arrow)
            {
                Handles.DrawLine(start, end);
                Handles.ConeHandleCap(0, end, Quaternion.LookRotation((end - start).normalized), 0.1f, EventType.Repaint);
            }
            else
            {
                Gizmos.DrawLine(start, end);
            }
        }

        public void MapFrom(object r)
        {
            Line fromR = (Line)r;
            start = fromR.start;
            end = fromR.end;
            arrow = fromR.arrow;
        }
    }

    [Serializable]
    public class Cube : IEquatable<Cube>, IScribbable
    {
        public Vector3 position;
        public Vector3 scale;
        public bool wireframe;

        public Cube(Vector3 _position, Vector3 _scale, bool as_wireframe = true)
        {
            position = _position;
            scale = _scale;
            wireframe = as_wireframe;
        }

        public static bool operator ==(Cube obj1, Cube obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Cube obj1, Cube obj2) => !(obj1 == obj2);

        public bool Equals(Cube other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (position == other.position && scale == other.scale);
        }
        public override bool Equals(object obj) => Equals(obj as Cube);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = position.GetHashCode();
                hashCode = (hashCode * 397) ^ scale.GetHashCode();
                return hashCode;
            }
        }

        public void Draw()
        {
            if (wireframe)
            {
                Gizmos.DrawWireCube(position, scale);
            }
            else
            {
                Gizmos.DrawCube(position, scale);
            }
        }
    }

    [Serializable]
    public class Sphere : IEquatable<Sphere>, IScribbable
    {
        public Vector3 position;
        public float scale;
        public bool wireframe;

        public Sphere(Vector3 _position, float _scale, bool as_wireframe = true)
        {
            position = _position;
            scale = _scale;
            wireframe = as_wireframe;
        }

        public static bool operator ==(Sphere obj1, Sphere obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Sphere obj1, Sphere obj2) => !(obj1 == obj2);

        public bool Equals(Sphere other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (position == other.position && scale == other.scale);
        }
        public override bool Equals(object obj) => Equals(obj as Sphere);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = position.GetHashCode();
                hashCode = (hashCode * 397) ^ scale.GetHashCode();
                return hashCode;
            }
        }

        public void Draw()
        {
            if (wireframe)
            {
                Gizmos.DrawWireSphere(position, scale);
            }
            else
            {
                Gizmos.DrawSphere(position, scale);
            }
        }
    }

    [Serializable]
    public class Label : IEquatable<Label>, IScribbable
    {
        public Vector3 position;
        public string text;

        public Label(Vector3 p, string t)
        {
            position = p;
            text = t;
        }

        public static bool operator ==(Label obj1, Label obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Label obj1, Label obj2) => !(obj1 == obj2);

        public bool Equals(Label other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (position == other.position && text == other.text);
        }
        public override bool Equals(object obj) => Equals(obj as Label);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = position.GetHashCode();
                hashCode = (hashCode * 397) ^ text.GetHashCode();
                return hashCode;
            }
        }

        public void Draw()
        {
            Handles.Label(position, new GUIContent(text));
        }
    }
}