using System;
using System.Net.Http;

namespace RvaJustin.AjaxGenerator.Attributes 
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AjaxAttribute : Attribute, IAjaxBehavior
    {
        
        public AjaxAttribute(string collection, string httpMethod)
        {
            Collection = collection;
            Method = new HttpMethod(httpMethod);
        }

        public string Collection { get; }
        
        public HttpMethod Method { get; }

        public override string ToString() 
            => string.Join("+", Method);

    }
}