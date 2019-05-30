using GrEmit;
using System.Reflection.Emit;

namespace Commentator.Example
{
    public class Generator
    {
        public static void GenerateAddition(DynamicMethod method)
        {
            using (var il = new GroboIL(method))
            {
                il.Ldarg(0);
                il.Ldarg(1);
                il.Add();
                il.Ret();
            }

        }
    }
}
