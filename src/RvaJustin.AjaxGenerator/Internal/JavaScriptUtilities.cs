using System.Linq;
using RvaJustin.AjaxGenerator.Exceptions;

namespace RvaJustin.AjaxGenerator.Internal
{
    internal static class JavaScriptUtilities
    {
        public static void CheckReservedWord(
            string objectName,
            string objectType)
        {
            if (!Constants.ReservedWords.Contains(objectName))
            {
                return;
            }
            
            throw new JavaScriptReservedWordException(objectType, objectName);
        }
    }
}