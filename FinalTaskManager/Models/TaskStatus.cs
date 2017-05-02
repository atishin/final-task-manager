using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalTaskManager.Models
{
    public class TaskStatus
    {
        [Key]
        public int Id { get; set; }
        public string StatusText { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}