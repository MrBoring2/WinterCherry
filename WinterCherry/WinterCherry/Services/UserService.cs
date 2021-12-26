using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterCherry.Data;

namespace WinterCherry.Services
{
    public class UserService
    {
        private static UserService instance;

        private UserService() { }
        public static UserService Instance => instance ?? (instance = new UserService());
        public Employee CurrentEmployee { get; private set; }
        public void SetEmployee(Employee employee)
        {
            if (employee != null && CurrentEmployee == null)
            {
                CurrentEmployee = employee;
            }
        }
        public void Logout()
        {
            CurrentEmployee = null;
        }
    }
}
