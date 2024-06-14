using UnityEngine;

namespace MapBattler
{
    public class Path : MonoBehaviour
    {
        [field: SerializeField] public Junction StartPoint { get; set; }
        [field: SerializeField] public Transform[] MidPoints { get; set; }
        [field: SerializeField] public Junction EndPoint { get; set; }
        [field: SerializeField] public bool Unlocked { get; set; }

        private Transform[] points;
        public Transform[] Points
        {
            get
            {
                if (points == null || points.Length != MidPoints.Length + 2)
                {
                    Points = new Transform[MidPoints.Length + 2];
                    Points[0] = StartPoint.transform;
                    for (int i = 0; i < MidPoints.Length; i++)
                    {
                        Points[i + 1] = MidPoints[i];
                    }
                    Points[^1] = EndPoint.transform;
                }
                return points;
            }
            private set => points = value;
        }

        private void Awake()
        {
            _ = Points;
        }

        private Junction ValidateJunction(Junction junction) => StartPoint != junction && EndPoint != junction ? junction : throw new System.Exception($"The given {typeof(Junction)} is not part of this path.");

        public Junction GetNextJunction(Junction current) => StartPoint == ValidateJunction(current) ? EndPoint : StartPoint;

        public Vector2 GetDirectionAtJunction(Junction current) => (Vector2)((StartPoint == ValidateJunction(current) ? MidPoints[0] : MidPoints[^1]).position - current.transform.position);

        public Vector2 Bezier(float t)
        {
            static int Combination(int n, int r) => Factorial(n) / (Factorial(r) * Factorial(n - r));
            static int Factorial(int n)
            {
                int nf = 1;
                while (n > 1)
                {
                    nf *= n;
                    n--;
                }
                return nf;
            }

            t = Mathf.Clamp01(t);
            Vector2 b = Vector2.zero;
            int n = Points.Length - 1;
            for (int i = 0; i <= n; i++)
            {
                b += Combination(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * (Vector2)Points[i % Points.Length].position;
            }
            return b;
        }

        [SerializeField] private bool drawPath = false;
        [SerializeField] private Color splineColor = Color.white;
        [SerializeField] private int resolution = 1000;
        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                float step = 1f / resolution;
                Vector2 previousPosition = Bezier(0);
                Gizmos.color = splineColor;
                for (int i = 0; i < resolution; i++)
                {
                    Vector2 position = Bezier(i * step);
                    Gizmos.DrawLine(previousPosition, position);
                    previousPosition = position;
                }
            }
        }
    }
}