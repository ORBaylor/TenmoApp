using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;
        
        
        public TenmoApp(string apiUrl)
        {
            this.tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }
        

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                // ViewCurrentBlance((int)tenmoApiService.UserId);

                //int userIdTest = tenmoApiService.UserId;
                // ViewCurrentBlance(tenmoApiService.UserId);
                //ShowTheUserBlance(tenmoApiService.Username);
                ViewCurrentBlance();
                //TODO pretty this shit up
            }

            if (menuSelection == 2)
            {
                // View your past transfers
                // ShowAccounts();
                //GetUserName(2006);
                //Console.WriteLine($"{tenmoApiService.Username}");
                //Account account = tenmoApiService.GetAccount(tenmoApiService.UserId);
                //int accountNum = account.accountId;
                //GetAccountName(2005);
                //Console.ReadLine();
                ViewAllTransfer();
                
            }

            if (menuSelection == 3)
            {
                // View your pending requests
                console.FeatureCommingSoon(tenmoApiService.Username);
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                //tenmoApiService.TransferMoney(1002, 50);
                BeginTransfer();
                //fix bug: can put in different userId
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
                console.FeatureCommingSoon(tenmoApiService.Username);
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }

        //public void ShowTheUserBlance(string username)
        //{
        //    int userId = tenmoApiService.UserId;
        //    int realUserIt = (int)ViewCurrentBlance(userId);
        //    Console.WriteLine($" Hey {username} your current blance is {userId} ");
        //    Console.ReadLine();
        //    console.Pause();
        //}
       
        private void ViewCurrentBlance()
        {
            Console.Clear();
            try
            {
                //TODO: pretty this shit up
                decimal currentBalance = tenmoApiService.GetAccountBlance(tenmoApiService.UserId);
                Console.Clear();
                Console.WriteLine("--------------------------");
                Console.WriteLine($"USER:     {tenmoApiService.Username}  ");
                Console.WriteLine($"BALANCE: ${currentBalance} ");
                //Console.ReadLine();
            }

            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void ShowAccounts()
        {
            List<Account> accounts = tenmoApiService.GetAllUserAndNames();
            Console.Clear();
            foreach (var account in accounts)
            {
                Console.WriteLine($" , {account.userId}  {account.username} ");
                Console.ReadLine();
            
            
            }
        }

        private string GetUserName(int accountId)
        {
            string UserName = tenmoApiService.GetUserNameFromAccount(accountId);
            return UserName;
            
        }
        private Account GetAccountName(int id)
        {
            Account account = tenmoApiService.GetAccount(id);
            Console.WriteLine();
            return account;
        }

        private void BeginTransfer()
        {
            List<Account> accounts = tenmoApiService.GetAllUserAndNames();

            Console.Clear();
            Console.WriteLine("ID  | USERNAME");
            Console.WriteLine("-----------------");
            foreach (var account in accounts)
            {

                Console.WriteLine($"{account.userId} | {account.username} ");



            }
            Console.WriteLine("-----------------");


            int UserId = console.PromptForAccountIdAmount();
            decimal amount = console.PromptForAmount();
            int RealUserId = tenmoApiService.UserId;
            decimal userBalance = tenmoApiService.GetAccountBlance(RealUserId);

            try
            {
                if (amount > userBalance || amount <= 0 || UserId == tenmoApiService.UserId || UserId.ToString().Length > 4 || UserId.ToString().Length < 4)
                {
                    console.PrintError("INVALID INPUT");
                    console.Pause();
                }
                else
                {
                    if (userBalance <= 0)
                    {
                        console.PrintError("TANSFER MUST BE A POSITIVE AMOUNT");
                    }
                    else
                    {
                        Console.Clear();
                        tenmoApiService.TransferMoney(UserId, amount);
                        // console.PrintSuccess("It Worked");
                        console.LoadingStatus(tenmoApiService.Username.ToUpper());
                       
                    }

                }
            }
            catch (NotSupportedException e)
            {

                 Console.WriteLine(e.Message);
            }

           

            //TODO pretty this shit up.
         
            //Console.ReadLine();

        
        
       }

        public void ViewAllTransfer()
        {

            try
            {
                Account account = tenmoApiService.GetAccount(tenmoApiService.UserId);
                int accountNumber = account.accountId;


                IList<Transfer> transfersList = tenmoApiService.GetTransferFromAccountNumber(accountNumber);
                Console.Clear();
                foreach (var transfer in transfersList)
                {

                    Console.WriteLine($"Transfer ID: {transfer.TransferId} Amount: {transfer.amount}");
                }
                //Console.ReadLine();
                Console.WriteLine("---------------------------------------");

                bool isDetails = false;
                while (!isDetails)
                {
                    int userpick = console.PromptForInteger("ENTER ID NUMBER TO SEE DETAILS | ENTER 0 TO RETURN TO MAIN MENU");
                    //metthod to show transfer detail.
                    if (userpick == 0)
                    {
                        isDetails = true;
                    }
                    else if(userpick.ToString().Length > 4 || userpick.ToString().Length < 4)
                    {
                        console.PrintError("OPTION OUT OF RANGE");
                    }
                    else
                    {
                       // Console.Clear();
                      
                        ShowTranferDetails(userpick);
                        Console.WriteLine("---------------------------------------");
                        console.Pause("PRESS ENTER TO RETURN TO THE MAIN MENU");
                        isDetails = true;
                    }
                   
                }

              
            }
            catch (NotSupportedException e)
            {

                Console.WriteLine(e.Message);
            }



        }

        public void ShowTranferDetails(int transferid)
        {
            Transfer transferDetails = tenmoApiService.GetAllDeatilsFromTransfer(transferid);
            try
            {
                Console.Clear();
                Console.WriteLine($" ACCOUNT FROM: {transferDetails.accountFrom}:");
                Console.WriteLine($" ACCOUNT TO: {transferDetails.accountTo}");
                Console.WriteLine("---------------------");
                Console.WriteLine($" AMOUNT TRANSFERED: ${transferDetails.amount}");

                
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine(e.Message);
            }
          


        }

    }
}
