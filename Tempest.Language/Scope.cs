using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tempest.Language
{
    public class Scope
    {
        private readonly Scope? m_Previous;

        private readonly Dictionary<string, Expression> m_Bindings = new();

        public Scope()
        {
        }

        private Scope(Scope previous)
        {
            m_Previous = previous;
        }

        public bool IsBound(string name)
        {
            if(m_Bindings.ContainsKey(name)) return true;
            if(m_Previous is not null) return m_Previous.IsBound(name);

            return false;
        }

        public Scope NewScope()
        {
            return new Scope(this);
        }
    }
}
