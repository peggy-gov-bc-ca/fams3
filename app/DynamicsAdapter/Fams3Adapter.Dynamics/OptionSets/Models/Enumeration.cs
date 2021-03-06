﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fams3Adapter.Dynamics.OptionSets.Models
{
    public class Enumeration : IComparable
    {

        public string Name { get; private set; }
        public int Value { get; private set; }

        public Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);
            var nameMatches = string.Equals(Name, otherValue.Name, StringComparison.OrdinalIgnoreCase);

            return typeMatches && valueMatches && nameMatches;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Value, Name).GetHashCode();
        }

        public int CompareTo(object other) => Value.CompareTo(((Enumeration)other).Value);

    }
}