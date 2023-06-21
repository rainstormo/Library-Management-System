﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToMemberMainPage;
        public event EventHandler<RoutedEventArgs> NavigateToAdminMainPage;
        public Member loggedInMember { get; set; }
        public LoginPage()
        {
            InitializeComponent();

            IDInput.KeyDown += LoginFeildsKeyDown;
            PINInput.KeyDown += LoginFeildsKeyDown;
        }
        public Account AuthenticateLoginDetails()
        {
            List<Account> accounts = FileManagement.LoadAccounts();
            string idInput = IDInput.Text;
            string pinInput = PINInput.Text;

            foreach (Account account in accounts)
            {
                if (account.id == idInput && account.pin == pinInput)
                {
                    return account;
                }
            }

            return null;
        }
        //checks the account type and Navigates them to their respective page
        private void HandleAuthentication(object sender, RoutedEventArgs e)
        {
            Account authenticatedAccount = AuthenticateLoginDetails();

            if (authenticatedAccount != null)
            {
                if (authenticatedAccount is Member member)
                {
                    loggedInMember = member;

                    NavigateToMemberMainPage?.Invoke(sender, e);
                }
                else if (authenticatedAccount is Admin)
                {
                    NavigateToAdminMainPage?.Invoke(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Cannot find account with those details");
            }
        }

        private void LoginFeildsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleAuthentication(sender, e);
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            HandleAuthentication(sender, e);
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
