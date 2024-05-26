using UnityEngine;

[AddComponentMenu("Path/Linear")]
public class LinearPath : Path
{
    public override void GetPoint(float t, out Vector3 position, out Quaternion? rotation)
    {
        t = Mathf.Clamp01(t);
        int max = points.Count + (closeLoop ? 1 : 0);

        float fl = 0; // full length
        for (int i = 1; i < max; i++)
        {
            fl += Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
        }
        float l = t % 1 * fl; // length

        int idx = -1;
        float sl = 0; // segment length
        float pl = 0; // partial length
        for (int i = 1; i < max && idx == -1; i++)
        {
            sl = Vector3.Distance(points[i - 1].position, points[i % points.Count].position);
            pl += sl;
            idx = (l - pl < 0) ? i : -1;
        }
        pl -= sl;

        float lt = (l - pl) / sl; // lerp value
        position = Vector3.Lerp(
              a: points[idx - 1].position,
              b: points[idx % points.Count].position,
              t: lt);
        rotation = Quaternion.Lerp(
               a: points[idx - 1].rotation,
               b: points[idx % points.Count].rotation,
               t: lt);
    }

    private void OnDrawGizmos()
    {
        if (PathIsValid)
        {
            Gizmos.color = pathColor;
            int n = points.Count + (closeLoop ? 1 : 0);
            for (int i = 1; i < n; i++)
            {
                Gizmos.DrawLine(points[i - 1].position, points[i % points.Count].position);
            } 
        }
    }
}
