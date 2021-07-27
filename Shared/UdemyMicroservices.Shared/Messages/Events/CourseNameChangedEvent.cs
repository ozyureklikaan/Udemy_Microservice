using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyMicroservices.Shared.Messages.Events
{
    public class CourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string UpdatedName { get; set; }
    }
}
