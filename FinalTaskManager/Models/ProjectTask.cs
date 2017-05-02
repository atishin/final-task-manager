using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalTaskManager.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ForeignKey("ClosedByUser")]
        public string ClosedByUserId { get; set; }
        public virtual ApplicationUser ClosedByUser { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [ForeignKey("TaskStatus")]
        public int TaskStatusId { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }


    }
}