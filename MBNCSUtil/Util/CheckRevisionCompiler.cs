using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Permissions;

namespace BNSharp.MBNCSUtil.Util
{
    internal static class CheckRevisionCompiler
    {
        private static readonly Dictionary<char, OpCode> Operators = new Dictionary<char, OpCode> 
        {
            { '+', OpCodes.Add },
            { '-', OpCodes.Sub },
            { '*', OpCodes.Mul }, 
            { '/', OpCodes.Div_Un },
            { '^', OpCodes.Xor },
            { '|', OpCodes.Or },
            { '&', OpCodes.And }
        };

        private static Dictionary<char, OpCode> StdArgumentList = new Dictionary<char, OpCode> 
        {
            { 'A', OpCodes.Ldarg_0 },
            { 'B', OpCodes.Ldarg_1 },
            { 'C', OpCodes.Ldarg_2 },
            { 'S', OpCodes.Ldarg_3 }
        };

        public static DynamicMethod Compile(string formulaName, IEnumerable<string> formulas)
        {
            Type parameterType = typeof(uint).MakeByRefType();

            DynamicMethod method = new DynamicMethod(formulaName, typeof(void), new Type[] { parameterType, parameterType, parameterType, parameterType }, Assembly.GetExecutingAssembly().ManifestModule);
            ILGenerator generator = method.GetILGenerator();

            foreach (string formula in formulas)
            {
                CompileStandardFormula(generator, formula);
            }

            generator.Emit(OpCodes.Ret);

            return method;
        }

        private static void CompileStandardFormula(ILGenerator generator, string formula)
        {
            char dest = formula[0];
            char opA = formula[2];
            char opB = formula[4];
            char op = formula[3];

            if (!StdArgumentList.ContainsKey(dest) || !StdArgumentList.ContainsKey(opA) || !StdArgumentList.ContainsKey(opB)
                || !Operators.ContainsKey(op))
                throw new ArgumentOutOfRangeException("formula", formula, "Unsupported or unrecognized operand or operator in revision check formula.");

            generator.Emit(StdArgumentList[dest]);
            generator.Emit(StdArgumentList[opA]);
            generator.Emit(OpCodes.Ldind_U4);
            generator.Emit(StdArgumentList[opB]);
            generator.Emit(OpCodes.Ldind_U4);
            generator.Emit(Operators[op]);
            generator.Emit(OpCodes.Stind_I4);
        }
    }
}
