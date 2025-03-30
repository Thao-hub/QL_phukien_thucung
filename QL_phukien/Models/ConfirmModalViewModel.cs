using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_phukien.Models
{
    public class ConfirmModalViewModel
    {
        public string ModalID { get; set; }
        public string FormID { get; set; }
        public string MessageID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; set; } // Changed to non-nullable
    }
}