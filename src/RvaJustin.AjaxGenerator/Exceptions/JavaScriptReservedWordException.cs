using System;

namespace RvaJustin.AjaxGenerator.Exceptions
{
    public class JavaScriptReservedWordException : Exception
    {
        public JavaScriptReservedWordException(string word, string name)
            : base($"{name} named \"{word}\" cannot be a JavaScript reserved word!")
        {

        }
    }
}