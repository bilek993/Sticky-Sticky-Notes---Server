using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sticky_Sticky_Notes___Server.Models
{
    public class ResultItem
    {
        public bool Successful { get; }
        public string ErrorMessage { get; }

        public ResultItem(bool successful, string errorMessage)
        {
            Successful = successful;
            ErrorMessage = errorMessage;
        }

        public ResultItem(bool successful)
        {
            Successful = successful;
            ErrorMessage = successful ? "OK" : "Unhandled exception.";
        }
    }
}