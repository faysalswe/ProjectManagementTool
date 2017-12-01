using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class ProjectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Status { get; set; }
        public int NumberOfMember { get; set; }
        public int NumberOfTask { get; set; }
    }
}