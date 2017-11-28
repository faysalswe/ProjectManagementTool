using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    [MetadataType(typeof(ProjectMetadata))]
    public partial class Project
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }

    public class ProjectMetadata
    {

    }
}