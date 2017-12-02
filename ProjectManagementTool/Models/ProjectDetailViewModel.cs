using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class ProjectDetailViewModel
    {
        public Project project { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}