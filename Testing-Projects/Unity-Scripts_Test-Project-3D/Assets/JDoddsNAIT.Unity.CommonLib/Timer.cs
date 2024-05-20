namespace JDoddsNAIT.Unity.CommonLib
{
    public struct Timer
    {
        private float time;

        public float Length { get; set; }
        public float Time
        {
            readonly get => time;
            set => time = value >= Length ? Length : value <= 0 ? 0 : value;
        }

        public Timer(float length) : this() => Length = length;

        public Timer(float length, float time) : this(length) => Time = time;

        /// <summary> Returns <see cref="Time"/> divided by <see cref="Length"/>. </summary>
        public readonly float Value => Time / Length;
        /// <summary> Returns true when <see cref="Time"/> is 0 or <see cref="Length"/>. </summary>
        public readonly bool Alarm => Time >= Length | Time <= 0;
    }
}
