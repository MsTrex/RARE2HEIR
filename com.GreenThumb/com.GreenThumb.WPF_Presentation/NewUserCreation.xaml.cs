﻿using com.GreenThumb.BusinessObjects;
using com.GreenThumb.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using com.GreenThumb.BusinessLogic.Interfaces;
using System.Text.RegularExpressions;

namespace com.GreenThumb.WPF_Presentation
{
    /// <summary>
    /// Interaction logic for NewUserCreation.xaml
    /// </summary>
    public partial class NewUserCreation : Window
    {
        private UserManager _userManagerObj = new UserManager();
        public NewUserCreation()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string fName = this.txtFName.Text;
            string lName = this.txtLName.Text;
            string username = this.txtnewUsername.Text;
            string password = this.txtnewPassword.Password;
            string passConfirm = this.txtPassConfirm.Password;
            bool isActive = true;
            bool isRegexMatch = true;
            try
            {
                if (!string.IsNullOrEmpty(fName) && !string.IsNullOrEmpty(lName) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passConfirm))
                {
                //    if (Regex.IsMatch(fName, @"(?i)^[a-z]+"))
                //        isRegexMatch = true;
                //    else
                //    {
                //        isRegexMatch = false;
                //        MessageBox.Show("Please enter only characters in first name");
                //    }

                //    if (Regex.IsMatch(lName, @"(?i)^[a-z]+"))
                //        isRegexMatch = true;
                //    else
                //    {
                //        isRegexMatch = false;
                //        MessageBox.Show("Please enter only characters in last name");
                //    }

                //    if (Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z].*[a-z]).{6}$"))
                //        isRegexMatch = true;
                //    else
                //    {
                //        isRegexMatch = false;
                //        MessageBox.Show("Password should contain 1 uppercase, 2 lowercase, 1 digit and a special character");
                //    }

                    if (isRegexMatch)
                    {
                        if (password != passConfirm)
                            MessageBox.Show("Passwords dont match!");
                        else
                        {
                            if (_userManagerObj.AddNewUser(fName, lName, string.Empty, string.Empty, username, password.HashSha256(), isActive, null) == 1)
                            {
                                MessageBox.Show("User has been created successfully!!");
                            }
                            else
                            {
                                MessageBox.Show("Username entered already exists. Please try a different username.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter all the fields");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            this.txtFName.Text = string.Empty;
            this.txtLName.Text = string.Empty;
            this.txtnewUsername.Text = string.Empty;
            this.txtnewPassword.Password = string.Empty;
            this.txtPassConfirm.Password = string.Empty;
        }
    }
}
