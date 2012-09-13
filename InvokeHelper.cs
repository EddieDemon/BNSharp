using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BNSharp
{
    internal delegate void Invokee<T>(Priority p, T args) where T : EventArgs;

    internal abstract class InvokeHelperBase
    {
        private Action<EventArgs> m_freeArgs;

        protected InvokeHelperBase(Action<EventArgs> freeArgumentsCallback)
        {
            m_freeArgs = freeArgumentsCallback;
        }

        public abstract void Invoke(Priority p);

        public abstract void FreeArguments();

        protected void FreeArguments(EventArgs args)
        {
            if (args != null)
            {
                m_freeArgs(args);
            }
        }
    }

    internal sealed class InvokeHelper<T> : InvokeHelperBase where T : EventArgs
    {
        public InvokeHelper(Invokee<T> target, T arguments, Action<EventArgs> freeArgumentCallback)
            : base(freeArgumentCallback)
        {
            Debug.Assert(target != null);

            Target = target;
            Arguments = arguments;
        }

        public Invokee<T> Target;
        public T Arguments;

        public override void Invoke(Priority p)
        {
            Target(p, Arguments);
        }

        public override void FreeArguments()
        {
            FreeArguments(Arguments);
        }
    }
}
