using System;

namespace BaseFrame.WebApi.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomAllowAnonyAttribute: Attribute
    {
    }
}