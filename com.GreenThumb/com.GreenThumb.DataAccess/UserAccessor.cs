﻿/// <summary>
/// Ryan Taylor
/// Created: 2016/02/26
/// Data Access methods relating to User objects
/// </summary>
/// <remarks>
/// Updated by Ryan Taylor 2016/03/03
/// </remarks>

using com.GreenThumb.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.GreenThumb.DataAccess
{
    public class UserAccessor
    {
        public static User RetrieveUserByUsername(string username)
        {
            User user;
            var conn = DBConnection.GetDBConnection();
            var query = @"Admin.spSelectUserByUsername";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Zip = reader.GetString(4),
                        EmailAddress = reader.GetString(5),
                        RegionId = reader.GetInt32(6),
                        Active = reader.GetBoolean(7)
                    };
                }
                else
                {
                    throw new ApplicationException("Data not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }

        public static int FindUserByUsernameAndPassword(string username, string password)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"Admin.spSelectUserWithUsernameAndPassword";

            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static int SetPasswordForUsername(string username, string oldPassword, string newPassword)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"Admin.spUpdatePassword";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@oldPassword", oldPassword);
            cmd.Parameters.AddWithValue("@newPassword", newPassword);

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static int InsertUser(User user)
        {
            int count = 0;

            // What comes first...a connection! Eureka!
            var conn = DBConnection.GetDBConnection();

            // What comes next is a command text
            string query = @"INSERT INTO Users " +
                           @"(FirstName, LastName, Phone, " +
                           @"EmailAddress, UserName, Password, RegionId ) " +
                           @"VALUES " +
                           @"('" + user.FirstName + "', '" + user.LastName +
                           @"', '" + user.Zip + "', '" + user.EmailAddress +
                           @"', '" + user.UserName + "', '" + user.Password + "','" + user.RegionId + ") ";

            // get a command object
            var cmd = new SqlCommand(query, conn);

            try
            {
                // open the connection
                conn.Open();

                // execute the command with ExecuteNonQuery()
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static List<Role> RetrieveRolesByUserID(int userID)
        {
            var roles = new List<Role>();
            var conn = DBConnection.GetDBConnection();

            var query = @"Admin.spSelectRoles";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            RoleID = reader.GetString(0),
                            Description = reader.GetString(1),
                            Active = reader.GetBoolean(2)
                        });
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        ///<summary>
        ///Author: Chris Schwebach
        ///UpdateUserPersonalInfo gets a database connection and updates information 
        ///in the DB where _accessToken.UserID = UserID 
        ///Date: 3/3/16
        ///</summary>
        public static int UpdateUserPersonalInfo(int userID, string firstName, string lastName, string zip, string emailAddress, int? regionId)
        {
            int rowCount = 0;

            // get a connection
            var conn = DBConnection.GetDBConnection();

            // we need a command object (the command text is in the stored procedure)
            var cmd = new SqlCommand("Admin.spUpdateUserPersonalInfo", conn);

            // set the command type for stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Zip", zip);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);

            if (regionId == null || regionId.Equals(""))
            {
                cmd.Parameters.AddWithValue("@regionID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@regionID", regionId);
            }

            cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int));
            cmd.Parameters["@RowCount"].Direction = ParameterDirection.ReturnValue;

            try
            {
                conn.Open();
                rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rowCount;
        }

        /// <summary>
        /// Rhett Allen
        /// Created: 2016/02/26
        /// 
        /// Edits the data fields for a user object in the database
        /// </summary>
        /// <param name="updateUser">The user that includes all of the updated fields</param>
        /// <param name="originalUser">The original user object to be checked for concurrency</param>
        /// <returns>A boolean based on if the user has been updated successfully</returns>
        public static bool EditUser(User updatedUser, User originalUser)
        {
            var conn = DBConnection.GetDBConnection();
            var query = "Admin.spUpdateUser";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", updatedUser.UserID);
            cmd.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
            cmd.Parameters.AddWithValue("@LastName", updatedUser.LastName);
            cmd.Parameters.AddWithValue("@Zip", updatedUser.Zip);
            cmd.Parameters.AddWithValue("@EmailAddress", updatedUser.EmailAddress);
            cmd.Parameters.AddWithValue("@UserName", updatedUser.UserName);
            cmd.Parameters.AddWithValue("@PassWord", updatedUser.Password);
            cmd.Parameters.AddWithValue("@Active", updatedUser.Active);
            cmd.Parameters.AddWithValue("@RegionID", updatedUser.RegionId);

            cmd.Parameters.AddWithValue("@OriginalFirstName", originalUser.FirstName);
            cmd.Parameters.AddWithValue("@OriginalLastName", originalUser.LastName);
            cmd.Parameters.AddWithValue("@OriginalZip", originalUser.Zip);
            cmd.Parameters.AddWithValue("@OriginalEmailAddress", originalUser.EmailAddress);
            cmd.Parameters.AddWithValue("@OriginalUserName", originalUser.UserName);
            cmd.Parameters.AddWithValue("@OriginalPassWord", originalUser.Password);
            cmd.Parameters.AddWithValue("@OriginalActive", originalUser.Active);
            cmd.Parameters.AddWithValue("@OriginalRegionID", originalUser.RegionId);

            bool updated = false;

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    updated = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return updated;
        }

        /// <summary>
        /// Rhett Allen
        /// Created: 2016/02/26
        /// 
        /// Get a single user based on the id in the database
        /// </summary>
        /// <param name="userID">The UserID in the database</param>
        /// <returns>The specified plant object</returns>
        public static User RetrieveUserByID(int userID)
        {
            User user = new User();

            var conn = DBConnection.GetDBConnection();
            var query = @"Admin.spSelectUser";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Zip = reader.GetString(3),
                        EmailAddress = reader.GetString(4),
                        UserName = reader.GetString(5),
                        Password = reader.GetString(6),
                        Active = reader.GetBoolean(7),
                        RegionId = reader.GetInt32(8)
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }

        ///<summary>
        ///Author: Chris Schwebach
        ///FetchUserPersonalInfo gets a database connection and retrieves user personal information 
        ///information in the DB where _accessToken.UserID = UserID 
        ///Date: 3/3/16
        ///</summary>
        public static List<User> FetchPersonalInfo(int userID)
        {

            var user = new List<User>();

            var conn = DBConnection.GetDBConnection();
            var query = @"Admin.spSelectUserPersonalInfo";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    User currentUser = new User()
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        Zip = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        RegionId = reader.GetInt32(4)

                    };
                    user.Add(currentUser);
                }
                else
                {
                    throw new ApplicationException("Data not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }      
       
    }
}
