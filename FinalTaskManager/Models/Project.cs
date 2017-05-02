using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalTaskManager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<string> Team { get; set; }

        [ForeignKey("Manager")]
        public string ManagerId { get; set; }
        public virtual ApplicationUser Manager { get; set; }

        public virtual ICollection<ProjectTask> Tasks { get; set; }

        public virtual ProjectChat ProjectChat { get; set; }
    }
}