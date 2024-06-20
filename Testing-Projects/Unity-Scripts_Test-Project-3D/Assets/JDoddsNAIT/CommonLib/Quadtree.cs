using UnityEngine;

public interface IOperable<TSelf> where TSelf : IOperable<TSelf>
{
    public TSelf Add(TSelf left, TSelf right);
    public TSelf Mul(TSelf left, float right);
}

public class Operable<TSelf, TValue> where TSelf : Operable<TSelf, TValue>, IOperable<TSelf>, new()
{
    public TValue Value { get; set; }

    public static implicit operator Operable<TSelf, TValue>(TValue value) => new TSelf() { Value = value };
    public static implicit operator TValue(Operable<TSelf, TValue> value) => value.Value;

    public static TSelf operator +(Operable<TSelf, TValue> left, Operable<TSelf, TValue> right) => ((TSelf)left).Add((TSelf)left, (TSelf)right);
    public static TSelf operator +(Operable<TSelf, TValue> operable) => operable * 1;

    public static TSelf operator -(Operable<TSelf, TValue> left, Operable<TSelf, TValue> right) => ((TSelf)left).Add((TSelf)left, -right);
    public static TSelf operator -(Operable<TSelf, TValue> operable) => operable * -1;

    public static TSelf operator *(Operable<TSelf, TValue> left, float right) => ((TSelf)left).Mul((TSelf)left, right);
    public static TSelf operator *(float left, Operable<TSelf, TValue> right) => right * left;

    public static TSelf operator /(Operable<TSelf, TValue> left, float right) => 1 / right * (TSelf)left;
}