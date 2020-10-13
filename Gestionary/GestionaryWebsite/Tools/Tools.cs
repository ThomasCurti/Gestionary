using GestionaryWebsite.DataAccess.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace GestionaryWebsite
{
    public static class Tools
    {
        public static string Hash(string password)
        {
            int SaltSize = 16;
            int KeySize = 32;
            Random rnd = new Random();
            int alea = rnd.Next(0, 512);
            using var algorithm = new Rfc2898DeriveBytes(
              password,
              SaltSize,
                 alea,
              HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{alea}.{salt}.{key}";
        }


        public static bool CheckPassword(string hash, string password)
        {
            var passValues = hash.Split(".");

            var alea = Convert.ToInt32(passValues[0]);
            var salt = Convert.FromBase64String(passValues[1]);
            var key = Convert.FromBase64String(passValues[2]);

            using var algorithm = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    alea,
                    HashAlgorithmName.SHA512);
            var keyToCheck = algorithm.GetBytes(32);
            var verified = keyToCheck.SequenceEqual(key);

            return verified;
        }

       
        public static string checkUser(string cookie, IEmployeeRepository employeeRepository, int AdminOrEmployee)
        {

            if (cookie == null)
                return null;

            var listClientUsers = employeeRepository.Get().Result;
            var roles = employeeRepository.GetRoles();

            var nbRoleAdmin = roles.Where(item => item.RoleName.ToLower().Contains("admin") || item.RoleName.ToLower().Contains("administrateur") || item.RoleName.ToLower().Contains("administrator")).FirstOrDefault();
            var nbRoleEmployee = roles.Where(item => item.RoleName.ToLower().Contains("employee") || item.RoleName.ToLower().Contains("employe") || item.RoleName.ToLower().Contains("employée") || item.RoleName.ToLower().Contains("employé")).FirstOrDefault();


            long userId = long.Parse(cookie);

            foreach (var item in listClientUsers)
            {
                if (item.Id == userId)
                {
                    if(AdminOrEmployee == 1 && (item.RoleId == nbRoleAdmin.RoleId || item.RoleId == nbRoleEmployee.RoleId))
                        return item.Username;
                           
                    if(AdminOrEmployee == 2 && item.RoleId == nbRoleAdmin.RoleId)
                        return item.Username;

                    break;
                }
            }
            return null;
        }
    }
}
