using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_UIAutomation.Models.UI
{
    public class EmployeeModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string ReportingManagerID { get; set; }
        public string JoiningDate { get; set; }
        public string EmploymentType { get; set; }
        public string IsEmployeeAdmin { get; set; }
        public string DoesEmployeeHaveReportee { get; set; }
    }
}
