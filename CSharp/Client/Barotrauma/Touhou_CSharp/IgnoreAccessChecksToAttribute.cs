using System;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class IgnoreAccessChecksToAttribute : Attribute
    {
        public IgnoreAccessChecksToAttribute(string assemblyName)
        {
        }
    }
}