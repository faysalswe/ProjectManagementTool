using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
        public string AssignedBy{ get; set; }
        public DateTime? DueDate{ get; set; }
    }
}