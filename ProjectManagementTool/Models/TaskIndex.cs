using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class TaskIndex
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<ProjectInfo> ProjectInfos { get; set; }
    }
}