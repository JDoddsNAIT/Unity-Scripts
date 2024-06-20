using UnityEngine;

public interface IOperable<TSelf, TValue, TScalar> where TSelf : IOperable<TSelf, TValue, TScalar>, new()
{
    public TValue Value { get; set; }

    // Methods
    public abstract TValue Add(TValue left, TValue right);

    public abstract TValue Sub(TValue left, TValue right);

    public abstract TValue Mul(TValue left, TScalar right);

    public abstract TValue Div(TValue left, TScalar right);


}

public abstract class Operable<TSelf, TValue, TScalar> : IOperable<TSelf, TValue, TScalar> where TSelf : Operable<TSelf, TValue, TScalar>, new()
{
    public abstract TValue Value { get; set; }

    public virtual TValue Add(TValue left, TValue right) => throw new System.NotImplementedException();
    public virtual TValue Div(TValue left, TScalar right) => throw new System.NotImplementedException();
    public virtual TValue Mul(TValue left, TScalar right) => throw new System.NotImplementedException();
    public virtual TValue Sub(TValue left, TValue right) => throw new System.NotImplementedException();

    // Operators
    public static implicit operator TValue(Operable<TSelf, TValue, TScalar> operable) => operable.Value;
    public static implicit operator Operable<TSelf, TValue, TScalar>(TValue vector3) => new TSelf() { Value = vector3 };

    public static TValue operator +(Operable<TSelf, TValue, TScalar> left, Operable<TSelf, TValue, TScalar> right) => left.Add(left.Value, right.Value);
    public static TValue operator +(Operable<TSelf, TValue, TScalar> left, TValue right) => left.Add(left.Value, right);

    public static TValue operator -(Operable<TSelf, TValue, TScalar> left, Operable<TSelf, TValue, TScalar> right) => left.Sub(left.Value, right.Value);
    public static TValue operator -(TValue left, Operable<TSelf, TValue, TScalar> right) => right.Sub(left, right.Value);

    public static TValue operator *(Operable<TSelf, TValue, TScalar> left, TScalar right) => left.Mul(left.Value, right);
    public static TValue operator *(TScalar left, Operable<TSelf, TValue, TScalar> right) => right.Mul(right.Value, left);

    public static TValue operator /(Operable<TSelf, TValue, TScalar> left, TScalar right) => left.Div(left.Value, right);
}

public class OperableQuaternion : Operable<OperableQuaternion, Quaternion, float>
{
    public override Quaternion Value { get; set; }

    public override Quaternion Add(Quaternion left, Quaternion right) => Quaternion.Euler(left.eulerAngles + right.eulerAngles);

    public override Quaternion Div(Quaternion left, float right) => Mul(left, 1 / right);

    public override Quaternion Mul(Quaternion left, float right) => Quaternion.Euler(left.eulerAngles * right);

    public override Quaternion Sub(Quaternion left, Quaternion right) => Add(left, Mul(right, -1));
}

public class VXF : Operable<VXF, Vector3, float>
{
    public override Vector3 Value { get; set; }

    public static implicit operator Vector3(VXF operable) => operable.Value;
    public static implicit operator VXF(Vector3 vector3) => new() { Value = vector3 };

    public override Vector3 Add(Vector3 left, Vector3 right) => left + right;

    public override Vector3 Div(Vector3 left, float right) => left / right;

    public override Vector3 Mul(Vector3 left, float right) => left * right;

    public override Vector3 Sub(Vector3 left, Vector3 right) => left - right;
}

public class VXQ : Operable<VXQ, Vector3, Quaternion>
{
    public override Vector3 Value { get; set; }
    public static implicit operator Vector3(VXQ operable) => operable.Value;
    public static implicit operator VXQ(Vector3 vector3) => new() { Value = vector3 };

    public override Vector3 Add(Vector3 left, Vector3 right) => left + right;

    public override Vector3 Mul(Vector3 left, Quaternion right) => right * left;

    public override Vector3 Sub(Vector3 left, Vector3 right) => left - right;
}
