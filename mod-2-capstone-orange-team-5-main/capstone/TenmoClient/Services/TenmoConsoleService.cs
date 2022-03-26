using System;
using System.Collections.Generic;
using TenmoClient.Models;
using System.Threading;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username.ToUpper()}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

       
       

        // Add application-specific UI methods here...

        public void FeatureCommingSoon(string username)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------");
           
            Console.WriteLine("PAID FEATURE COMMING SOON");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"THANK YOU FOR YOUR PATIENTS {username.ToUpper()}");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("YOU WILL REDIRECTED TO THE MAIN MENU IN: 3");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("------------------------------");
            // Console.WriteLine($"THANK YOU FOR YOUR PATIENTS {username}");
            Console.WriteLine("YOU WILL REDIRECTED TO THE MAIN MENU IN: 2");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("------------------------------");
           // Console.WriteLine($"THANK YOU FOR YOUR PATIENTS {username}");
            Console.WriteLine("YOU WILL REDIRECTED TO THE MAIN MENU IN: 1");
            Thread.Sleep(1000);
            Console.Clear();
            Console.ResetColor();

        }

        public void LoadingStatus(string username)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("TRANSFER IN PROGRESS:----");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("TRANSFER IN PROGRESS:---------");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("TRANSFER IN PROGRESS:-----------------");
            Thread.Sleep(1000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            PrintSuccess($"CONGRATULATIONS {username.ToUpper()} TRANSFER STATUS: --APPROVED--");
            Thread.Sleep(1500);
            Console.ResetColor();



        }
    }
}
