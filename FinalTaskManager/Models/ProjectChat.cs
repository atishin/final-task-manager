using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalTaskManager.Models
{
    public class ProjectChat
    {
        [Key, ForeignKey("Project")]
        public int Id { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}