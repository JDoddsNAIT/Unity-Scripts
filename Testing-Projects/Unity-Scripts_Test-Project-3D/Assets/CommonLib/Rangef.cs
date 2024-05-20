namespace JDoddsNAIT.Unity.CommonLib
{
    public struct Range
    {
        private float min, max;

        public float Min { readonly get => min < max ? min : max; set => min = value; }
        public float Max { readonly get => max > min ? max : min; set => max = value; }

        public Range(float max) : this() => Max = max;

        public Range(float min, float max) : this(max) => Min = min;

        public static implicit operator Range(float max) => new(max);
        public static explicit operator Range(double max) => new((float)max);

        /// <summary>
        /// Interpolates between <see cref="Min"/> and <see cref="Max"/> using <paramref name="value"/>.
        /// </summary>
        public readonly float Lerp(float value) => Min + (Max - Min) * value;
        /// <summary>
        /// Interpolates between <paramref name="range"/>'s <see cref="Min"/> and <see cref="Max"/> using <paramref name="value"/>.
        /// </summary>
        public static float Lerp(Range range, float value) => range.Lerp(value);

        /// <summary> 
        /// Returns <paramref name="value"/> as a percentage between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public readonly float Normal(float value) => (value - Min) / (Max - Min);
        /// <summary> 
        /// Returns <paramref name="value"/> as a percentage between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public static float Normal(Range range, float value) => range.Normal(value);

        /// <summary>
        /// Clamps a given <paramref name="value"/> to be within <see cref="Range"/>.
        /// </summary>
        public readonly float Clamp(float value) => value >= Max ? Max : value <= Min ? Min : value;
        /// <summary>
        /// Clamps a given <paramref name="value"/> to be within <paramref name="range"/>.
        /// </summary>
        public static float Clamp(float value, Range range) => range.Clamp(value);

        /// <summary>
        /// Returns true if <paramref name="value"/> is greater than <see cref="Min"/> and less than <see cref="Max"/>.
        /// </summary>
        public readonly bool Contains(float value) => Min < value && value < Max;
        /// <summary>
        /// Returns true if <paramref name="value"/> is greater than <see cref="Min"/> and less than <see cref="Max"/>.
        /// </summary>
        public static bool Contains(Range range, float value) => range.Contains(value);

        /// <summary>
        /// Returns true if <paramref name="value"/> is greater than or equal to <see cref="Min"/> and less than or equal to <see cref="Max"/>.
        /// </summary>
        public readonly bool Includes(float value) => Min <= value & value <= Max;
        /// <summary>
        /// Returns true if <paramref name="value"/> is greater than or equal to <see cref="Min"/> and less than or equal to <see cref="Max"/>.
        /// </summary>
        public static bool Includes(Range range, float value) => range.Includes(value);
    }
}