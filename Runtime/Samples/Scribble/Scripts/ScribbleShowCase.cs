using System;
using System.Collections;
using System.Collections.Generic;
using DollUtil.Buggy.Scribble;
using UnityEngine;

//example of a custom Scribbable object
//must impliment IScribbable and ideally IEquatable so that you can += and -= effectively
[Serializable]
public class ColorSphere : IEquatable<ColorSphere>, IScribbable
{
    public Vector3 position;
    public Color color;
    public float scale;
    public bool wireframe;

    public ColorSphere(Vector3 _position, float _scale, Color _color, bool as_wireframe = true)
    {
        position = _position;
        scale = _scale;
        color = _color;
        wireframe = as_wireframe;
    }

    public static bool operator ==(ColorSphere obj1, ColorSphere obj2)
    {
        if (ReferenceEquals(obj1, obj2))
            return true;
        if (ReferenceEquals(obj1, null))
            return false;
        if (ReferenceEquals(obj2, null))
            return false;
        return obj1.Equals(obj2);
    }

    public static bool operator !=(ColorSphere obj1, ColorSphere obj2) => !(obj1 == obj2);

    public bool Equals(ColorSphere other)
    {
        if (ReferenceEquals(other, null))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return (position == other.position && scale == other.scale);
    }
    public override bool Equals(object obj) => Equals(obj as ColorSphere);

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
        Gizmos.color = color;
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

public class ScribbleShowCase : MonoBehaviour
{
    public Sphere moving_sphere = new Sphere(Vector3.zero, 0.2f);
    public ColorSphere moving_sphere2 = new ColorSphere(Vector3.zero, 0.2f, Color.red);
    private float time = 10;
    public Label time_label = new Label(Vector3.zero, "");

    // Start is called before the first frame update
    void Start()
    {
        //+= Adds object to be drawn. Can be removed later used -=
        Scribble.Draw += moving_sphere;
        Scribble.Draw += moving_sphere2;
        Scribble.Draw += time_label;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            time_label.text = time.ToString("F2");

            moving_sphere.position = Vector3.right * 2 * Mathf.Sin(Time.time);
            //*= adds an object to a stack thats cleared each frame
            Scribble.Draw *= new Label(moving_sphere.position + Vector3.up, (moving_sphere.position.x > 0) ? "Right" : "Left");

            moving_sphere2.position = Vector3.up * 2 * Mathf.Sin(Time.time);
            Scribble.Draw *= new Label(moving_sphere2.position + Vector3.up, (moving_sphere2.position.y > 0) ? "Top" : "Bottom");

            Scribble.Draw *= new Line(new Vector3(moving_sphere.position.x, moving_sphere2.position.y, 0), moving_sphere.position, true);
            Scribble.Draw *= new Line(new Vector3(moving_sphere.position.x, moving_sphere2.position.y, 0), moving_sphere2.position, true);
        }
        else
        {
            Scribble.Draw -= moving_sphere;
            Scribble.Draw -= moving_sphere2;
        }
    }
}
