using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class AssignResourceViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ResourcePerson { get; set; }
        public string Designation { get; set; }
    }
}