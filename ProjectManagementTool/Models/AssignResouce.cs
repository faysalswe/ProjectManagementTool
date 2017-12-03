using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementTool.Models
{
    [MetadataType(typeof(AssignResouceMetadata))]
    public partial class AssignResouce
    {
    }
    public  class AssignResouceMetadata
    {
        [Remote("IsRecordAvailable", "AssignResources",ErrorMessage = "User is already assign in this project")]
        public int ProjectId{ get; set; }
    }
}