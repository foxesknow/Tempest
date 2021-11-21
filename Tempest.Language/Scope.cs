using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Tempest.Language
{
    public class Scope
    {
        private readonly Scope? m_Previous;

        private readonly Dictionary<string, object?> m_Bindings = new();

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

        public void Bind(string name, object? value)
        {
            m_Bindings.Add(name, value);
        }

        public bool TryGetBinding(string name, out object? binding)
        {
            if(m_Bindings.TryGetValue(name, out binding))
            {
                return true;
            }

            if(m_Previous is not null)
            {
                return m_Previous.TryGetBinding(name, out binding);
            }

            return false;
        }

        public Scope BeginScope()
        {
            return new Scope(this);
        }
    }
}
