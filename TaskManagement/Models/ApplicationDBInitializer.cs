using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManagement.Models
{
    
    public class ApplicationDBInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            IList<Status> defaultStatus = new List<Status>();

            defaultStatus.Add(new Status() { Name = "New" });
            defaultStatus.Add(new Status() { Name = "In Progress" });
            defaultStatus.Add(new Status() { Name = "Finished" });

            context.Status.AddRange(defaultStatus);

            base.Seed(context);
        }
    }
}