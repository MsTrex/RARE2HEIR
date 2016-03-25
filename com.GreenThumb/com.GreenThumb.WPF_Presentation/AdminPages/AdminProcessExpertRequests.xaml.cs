﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using com.GreenThumb.BusinessObjects;
using com.GreenThumb.BusinessLogic;

namespace com.GreenThumb.WPF_Presentation.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminProcessExpertRequests.xaml
    /// Created By: Trent Cullinan 03/15/2016
    /// </summary>
    public partial class AdminProcessExpertRequests : Page
    {
        private const string ADMIN = "Admin";

        private AccessToken accessToken
            = null;
        private AdminExpertRequestsManager adminExpertRequestsManager
            = null;

        /// <summary>
        /// Constructor that cannot be used unless accessToken has an admin role.
        /// 
        /// Created By: Trent Cullinan 03/15/2016
        /// </summary>
        /// <param name="accessToken"></param>
        public AdminProcessExpertRequests(AccessToken accessToken)
        {
            if (CheckAdminRoleStatus(accessToken))
            {
                InitializeComponent();

                try
                {
                    adminExpertRequestsManager
                        = new AdminExpertRequestsManager(accessToken);
                }
                catch (Exception ex)
                {
                    lblResponseMessage.Content = ErrorMessage(ex.Message);
                }

                this.accessToken = accessToken;
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void btnActionOne_Click(object sender, RoutedEventArgs e)
        {
            if (tiRequests.IsSelected)
            {
                var request = (ExpertRequest)dgExpertRequests.SelectedItem;

                if (null != request)
                {
                    try
                    {
                        if (adminExpertRequestsManager.ApproveRequest(
                            this.accessToken, request))
                        {
                            BindExpertRequests();

                            lblResponseMessage.Content = "Request approved.";

                            ClearRequestContent();
                        }
                        else
                        {
                            lblResponseMessage.Content = ErrorMessage("Request failed to approve.");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblResponseMessage.Content = ErrorMessage(ex.Message);
                    }
                }
                else
                {
                    lblResponseMessage.Content = ErrorMessage("No request selected.");
                }
            }
            else if (tiAllUsers.IsSelected)
            {
                var user = (User)dgAllUsers.SelectedItem;

                if (null != user)
                {
                    try
                    {
                        if (adminExpertRequestsManager.PromoteUser(
                            this.accessToken, user))
                        {
                            BindCurrentUsers();

                            lblResponseMessage.Content = "User promoted.";
                        }
                        else
                        {
                            lblResponseMessage.Content = ErrorMessage("Failed to promote user.");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblResponseMessage.Content = ErrorMessage(ex.Message);
                    }
                }
                else
                {
                    lblResponseMessage.Content = ErrorMessage("No user selected.");
                }
            }
            else if (tiCurrentExperts.IsSelected)
            {
                var expert = (User)dgCurrentExperts.SelectedItem;

                if (null != expert)
                {
                    try
                    {
                        if (adminExpertRequestsManager.DemoteExpert(
                            this.accessToken, expert))
                        {
                            BindCurrentExperts();

                            lblResponseMessage.Content = "Expert Demoted.";
                        }
                        else
                        {
                            lblResponseMessage.Content = ErrorMessage("Request failed to decline.");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblResponseMessage.Content = ErrorMessage(ex.Message);
                    }
                }
                else
                {
                    lblResponseMessage.Content = ErrorMessage("No expert selected.");
                }
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void btnActionTwo_Click(object sender, RoutedEventArgs e)
        {
            if (tiRequests.IsSelected)
            {
                var request = (ExpertRequest)dgExpertRequests.SelectedItem;

                if (null != request)
                {
                    try
                    {
                        if (adminExpertRequestsManager.DeclineRequest(
                            this.accessToken, request))
                        {
                            BindExpertRequests();

                            lblResponseMessage.Content = "Request declined.";

                            ClearRequestContent();
                        }
                        else
                        {
                            lblResponseMessage.Content = ErrorMessage("Request failed to decline.");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblResponseMessage.Content = ErrorMessage(ex.Message);
                    }
                }
                else
                {
                    lblResponseMessage.Content = ErrorMessage("No request selected.");
                }
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void btnSearchUsers_Click(object sender, RoutedEventArgs e)
        {
            dgAllUsers.ItemsSource
                = adminExpertRequestsManager.SearchUsers(txtSearchUsers.Text);

            txtSearchUsers.Clear();
        }

        // Created By: Trent Cullinan 03/15/2016
        private void btnSearchExperts_Click(object sender, RoutedEventArgs e)
        {
            dgCurrentExperts.ItemsSource
                = adminExpertRequestsManager.SearchExperts(txtSearchExperts.Text);

            txtSearchExperts.Clear();
        }

        // Created By: Trent Cullinan 03/15/2016
        private void tcScreens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tiRequests.IsSelected)
            {
                btnActionTwo.Visibility = Visibility.Visible;
                btnActionTwo.Content = "Decline";
                btnActionOne.Content = "Approve";

                BindExpertRequests(refresh: true);
            }
            else if (tiAllUsers.IsSelected)
            {
                btnActionTwo.Visibility = Visibility.Hidden;
                btnActionTwo.Content = string.Empty;
                btnActionOne.Content = "Set as Expert";

                BindCurrentUsers(refresh: true);
            }
            else if (tiCurrentExperts.IsSelected)
            {
                btnActionTwo.Visibility = Visibility.Hidden;
                btnActionTwo.Content = string.Empty;
                btnActionOne.Content = "Remove Expert";

                BindCurrentExperts(refresh: true);
            }

            lblResponseMessage.Content = string.Empty;

            ClearRequestContent();
        }

        // Created By: Trent Cullinan 03/15/2016
        private void dgExpertRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            var request = (ExpertRequest)dgExpertRequests.SelectedItem;

            if (null != request)
            {
                lblRequestTitle.Content
                    = request.RequestTitle;
                tbRequestContent.Text
                    = request.RequestContent;
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void dgAllUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            lblResponseMessage.Content = string.Empty;
        }

        // Created By: Trent Cullinan 03/15/2016
        private void dgCurrentExperts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            lblResponseMessage.Content = string.Empty;
        }

        // Created By: Trent Cullinan 03/15/2016
        private void BindExpertRequests(bool refresh = false)
        {
            try
            {
                dgExpertRequests.ItemsSource
                    = adminExpertRequestsManager.RetrieveExpertRequests(this.accessToken, refresh);
                dgExpertRequests.Items.Refresh();
            }
            catch (Exception ex)
            {
                lblResponseMessage.Content = ErrorMessage(ex.Message);
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void BindCurrentUsers(bool refresh = false)
        {
            try
            {
                dgAllUsers.ItemsSource
                    = adminExpertRequestsManager.RetrieveAllUsers(this.accessToken, refresh);
                dgAllUsers.Items.Refresh();
            }
            catch (Exception ex)
            {
                lblResponseMessage.Content = ErrorMessage(ex.Message);
            }
        }

        // Created By: Trent Cullinan 03/15/2016
        private void BindCurrentExperts(bool refresh = false)
        {
            try
            {
                dgCurrentExperts.ItemsSource
                    = adminExpertRequestsManager.RetrieveAllExperts(this.accessToken, refresh);
                dgCurrentExperts.Items.Refresh();
            }
            catch (Exception ex)
            {
                lblResponseMessage.Content = ErrorMessage(ex.Message);
            }
        }

        // Created By: Trent Cullinan 02/24/2016
        private bool CheckAdminRoleStatus(AccessToken accessToken)
        {
            bool flag = false;

            if (null != accessToken)
            {
                flag = 0 < accessToken.Roles.Where(r => r.RoleID.Equals(ADMIN)).Count();
            }

            return flag;
        }

        // Created By: Trent Cullinan 02/24/2016
        private string ErrorMessage(string message)
        {
            return "Error: " + message;
        }

        // Created By: Trent Cullinan 02/24/2016
        private void ClearRequestContent()
        {
            lblRequestTitle.Content
                = string.Empty;
            tbRequestContent.Text
                = string.Empty;
        }

        // Created By: Trent Cullinan 02/24/2016
        private void txtSearchUsers_GotFocus(object sender, RoutedEventArgs e)
        {
            btnSearchUsers.IsDefault = true;
        }

        // Created By: Trent Cullinan 02/24/2016
        private void txtSearchUsers_LostFocus(object sender, RoutedEventArgs e)
        {
            btnSearchUsers.IsDefault = false;
        }

        // Created By: Trent Cullinan 02/24/2016
        private void txtSearchExperts_GotFocus(object sender, RoutedEventArgs e)
        {
            btnSearchExperts.IsDefault = true;
        }

        // Created By: Trent Cullinan 02/24/2016
        private void txtSearchExperts_LostFocus(object sender, RoutedEventArgs e)
        {
            btnSearchExperts.IsDefault = false;
        }

    }
}