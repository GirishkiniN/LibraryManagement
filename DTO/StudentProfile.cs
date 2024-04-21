using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    [Keyless]
    public class StudentProfile
    {
        public string StudentName { get; set; }
        public string USN { get; set; }
        public int TotalBooks { get; set; }
        public int NotReturned { get; set; }
    }
}
