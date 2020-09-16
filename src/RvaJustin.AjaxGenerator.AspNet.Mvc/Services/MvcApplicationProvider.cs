using System;
using System.Reflection;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class MvcApplicationProvider
    {
        public Type BaseControllerType { get; }
        public object App { get; }

        public Assembly Assembly => App.GetType().Assembly;

        public MvcApplicationProvider(object httpApplication, Type baseControllerType)
        {
            BaseControllerType = baseControllerType;
            App = httpApplication;
        }
    }
}