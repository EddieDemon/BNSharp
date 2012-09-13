using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Threading;
using System.IO;

namespace BNSharp.MBNCSUtil.Util
{
    internal static class CheckRevisionFormulaTracker
    {
        private const int MAX_CURRENT_FORMULAS = 5; 

        private static Dictionary<string, DynamicMethod> ActiveFormulas = new Dictionary<string, DynamicMethod>();
        private static Dictionary<string, StandardCheckRevisionImplementation> ActiveFormulaCallbacks = new Dictionary<string, StandardCheckRevisionImplementation>();
        private static Queue<string> FormulaList = new Queue<string>();

        public static StandardCheckRevisionImplementation GetImplementation(IEnumerable<string> formulas)
        {
            string completeText = JoinWith(formulas, ' ');
            StandardCheckRevisionImplementation impl = null;
            if (!ActiveFormulaCallbacks.TryGetValue(completeText, out impl))
            {
                DynamicMethod method = CheckRevisionCompiler.Compile(completeText, formulas);
                impl = method.CreateDelegate(typeof(StandardCheckRevisionImplementation)) as StandardCheckRevisionImplementation;
                FormulaList.Enqueue(completeText);
                ActiveFormulas.Add(completeText, method);
                ActiveFormulaCallbacks.Add(completeText, impl);

                if (FormulaList.Count > MAX_CURRENT_FORMULAS)
                {
                    ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                    {
                        string formulaToRelease = FormulaList.Dequeue();

                        if (ActiveFormulas.ContainsKey(formulaToRelease))
                            ActiveFormulas.Remove(formulaToRelease);

                        if (ActiveFormulaCallbacks.ContainsKey(formulaToRelease))
                            ActiveFormulaCallbacks.Remove(formulaToRelease);
                    });
                }
            }

            return impl;
        }

        private static string JoinWith(IEnumerable<string> strings, char joiner)
        {
            List<string> source = new List<string>(strings);
            StringBuilder sb = new StringBuilder();
            if (source.Count > 0)
                sb.Append(source[0]);
            for (int i = 1; i < source.Count; i++)
            {
                sb.Append(joiner);
                sb.Append(source[i]);
            }

            return sb.ToString();
        }

        public static void InitializeValues(string valueString, List<string> formulas, out uint A, out uint B, out uint C)
        {
            A = 0;
            B = 0;
            C = 0;

            string[] tokens = valueString.Split(' ');
            foreach (string token in tokens)
            {
                if (token.Length > 3 && token[1] == '=')
                {
                    if (char.IsDigit(token[2]))
                    {
                        string value = token.Substring(2);
                        uint result = 0;
                        if (uint.TryParse(value, out result))
                        {
                            if (token[0] == 'A')
                                A = result;
                            else if (token[0] == 'B')
                                B = result;
                            else if (token[0] == 'C')
                                C = result;
                        }
                    }
                    else
                    {
                        formulas.Add(token);
                    }
                }
            }
        }

        internal static void FileAppendBytes(string path, byte[] bytes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
