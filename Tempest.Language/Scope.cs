using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Tempest.Language
{
    public class Scope
    {
        private readonly Dictionary<Identifier, object?> m_Bindings = new();

        public Scope()
        {
            this.Previous = null;
        }

        private Scope(Scope previous)
        {
            this.Previous = previous;
        }

        private Scope? Previous{get;}

        private IReadOnlyDictionary<Identifier, object?> Bindings
        {
            get{return m_Bindings;}
        }

        public bool IsBound(Identifier name)
        {
            if(name.IsInvalid) throw new LanguageException("invalid name");

            for(var scope = this; scope is not null; scope = scope.Previous)
            {
                if(scope.Bindings.ContainsKey(name)) return true;
            }
            
            return false;
        }

        public void Bind(Identifier name, object? value)
        {
            if(name.IsInvalid) throw new LanguageException("invalid name");
            if(m_Bindings.ContainsKey(name)) throw new LanguageException($"name is already bound: {name}");

            m_Bindings.Add(name, value);
        }

        public bool TryGetBinding(Identifier name, out object? binding)
        {
            if(name.IsInvalid) throw new LanguageException("invalid name");

            for(var scope = this; scope is not null; scope = scope.Previous)
            {
                if(scope.Bindings.TryGetValue(name, out binding)) return true;
            }

            binding = null;
            return false;
        }

        public Scope BeginScope()
        {
            return new Scope(this);
        }
    }
}
