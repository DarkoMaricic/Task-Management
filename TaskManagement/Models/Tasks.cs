using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagement.Models {
    public class Tasks {

        public int Id { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Value is not in range")]
        public float Progress { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

    }
}