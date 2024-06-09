using System;
using System.Collections;

namespace JDoddsNAIT.Unity.CommonLib
{
    public struct SyncTimer
    {
        private float time;

        public float Length { get; set; }
        public float Time
        {
            readonly get => time;
            set => time = Range.Clamp(value, Length);
        }

        public SyncTimer(float length) : this() => Length = length;

        public SyncTimer(float length, float time) : this(length) => Time = time;

        /// <summary> Returns <see cref="Time"/> divided by <see cref="Length"/>. </summary>
        public readonly float Value => Time / Length;
        /// <summary> Returns true when <see cref="Time"/> is 0 or <see cref="Length"/>. </summary>
        public readonly bool Alarm => Time >= Length | Time <= 0;
    }

    public struct AsyncTimer
    {
        public float Length { get; set; }
        public float Time { get; set; }

        public AsyncTimer(float length) : this() => Length = length;

        public AsyncTimer(float length, float time) : this(length) => Time = time;

        public IEnumerator Start(Action<float> callback) => Start(callback, null);
        public IEnumerator Start(Action alarm) => Start(null, alarm);
        public IEnumerator Start(Action<float> callback, Action alarm)
        {
            Time = 0;
            while (Time < Length)
            {
                callback(Time);
                Time += UnityEngine.Time.deltaTime;
                yield return null;
            }
            alarm();
        }
    }
}
