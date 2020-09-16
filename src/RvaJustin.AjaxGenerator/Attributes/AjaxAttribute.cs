using System;
using System.Linq;
using System.Net.Http;

namespace RvaJustin.AjaxGenerator.Attributes 
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AjaxAttribute : Attribute
    {
        
        #region Constructors and Destructors

        public AjaxAttribute(string collection, string httpMethod)
        {
            Collection = collection;
            Methods = new[] { new HttpMethod(httpMethod) };
        }

        #endregion

        #region Public Properties
        
        public string Collection { get; }
        
        public HttpMethod[] Methods { get; }

        #endregion

        #region Public Methods and Operators

        public override string ToString() 
            => string.Join("+", Methods.Select(m => m.Method));

        #endregion
    }
}