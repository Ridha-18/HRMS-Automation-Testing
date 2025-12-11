using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_UIAutomation.Models.UI;
using Bogus;

namespace HRMS_UIAutomation.TestData
{
    public static class EmployeeDataFactory
    {
        public static EmployeeModel GenerateEmployee()
        {
            var faker = new Faker("en");

            var model = new EmployeeModel
            {
                FullName = faker.Name.FullName(),
                Email = faker.Internet.Email(),
                Phone = faker.Phone.PhoneNumber("##########"),
                Password = "Test@123", // You can also generate dynamically
                Department = "QA", // Or randomize: faker.PickRandom(new[] { "QA", "Dev", "HR" })
                ReportingManagerID = "2",
                JoiningDate = DateTime.Now.ToString("dd-MM-yyyy"),
                EmploymentType = "Full-Time",
                IsEmployeeAdmin = "Yes",
                DoesEmployeeHaveReportee = "No"
            };

            return model;
        }
    }
}
