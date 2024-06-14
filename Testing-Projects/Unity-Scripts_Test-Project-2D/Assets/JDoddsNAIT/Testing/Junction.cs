using System.Linq;
using UnityEngine;

namespace MapBattler
{
    public class Junction : MonoBehaviour
    {
        [field: SerializeField] public Battle Battle { get; set; }
        [field: SerializeField] public Path[] Paths { get; set; }
        [field: SerializeField] public bool Unlocked { get; set; } = false;

        [ContextMenu("Find Paths")]
        public void FindPaths() => Paths = FindObjectsOfType<Path>().Where(p => p.StartPoint == this || p.EndPoint == this).ToArray();

        private void Awake()
        {
            FindPaths();
        }

        public void UnlockPaths()
        {
            Unlocked = true;
            Path[] paths = Paths.Where(p  => !p.Unlocked).ToArray();
            foreach (var path in paths)
            {
                path.Unlocked = true;
                Junction next = path.GetNextJunction(this);
                if (next != null && next.Battle != null)
                {
                    next.UnlockPaths();
                }
            }
            return;
        }

        public Path GetInputPath(Vector2 inputDirection) =>
            Paths.Where(p => p.Unlocked).OrderBy(p => Vector2.Angle(p.GetDirectionAtJunction(this), inputDirection)).First();
    }
}