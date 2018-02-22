using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class ValidationModel
    {
        public string ErrorType { get; set; }
        public string Message { get; set; }

        public List<object> GetErrorsNullType(object[] objectToTest)
        {
            List<object> Errors = new List<object>();
            foreach(object element in objectToTest)
            {
                if (element is null)
                {
                    Errors.Add(new { Message = ""});
                }
            }
            return Errors;
        }
    }
}
