
using System.Reflection;
using System.Runtime.CompilerServices;

var source = typeof(C1).GetMethod("Method1");
var dest = typeof(C2).GetMethod("Method2");

IllegalOverride(source, dest);

var c1 = new C1();
c1.Method1();

unsafe void IllegalOverride(MethodBase source, MethodBase dest)
{
    RuntimeHelpers.PrepareMethod(source.MethodHandle);
    RuntimeHelpers.PrepareMethod(dest.MethodHandle);

    var fp1 = source.MethodHandle.GetFunctionPointer();
    var fp2 = dest.MethodHandle.GetFunctionPointer();

    var f1Ptr = (byte*)fp1.ToPointer();
    var f2Ptr = (byte*)fp2.ToPointer();
    var sJump = (uint)f1Ptr + 5;

    *(uint*)(f1Ptr + 1) = (uint)(f2Ptr - sJump);
}

sealed class C1
{
    public void Method1()
    {
        Console.WriteLine("Method1");
    }
}

sealed class C2
{
    public void Method2()
    {
        Console.WriteLine("System is hacked");
    }
}