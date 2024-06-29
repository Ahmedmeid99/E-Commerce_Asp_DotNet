using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography;

namespace E_Commerce_BusniessLayer.Global
{
    public static class Utility
    {
        public static string HashingPassword(string Password)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {

                byte[] hashBytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
