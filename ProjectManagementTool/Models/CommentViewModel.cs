using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string CommenterName { get; set; }
        public DateTime? DateTime { get; set; }
    }
}