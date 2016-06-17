using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DataAccess;
using Common.Models;

namespace Common.Logic
{
    public class UserLogic
    {
        public UserLogic()
        {
            
        }

        public User[] GetUserList()
        {
            User[] result = null;

            UserDataAccess uda = new UserDataAccess();

            result = uda.GetAllUsers().ToArray<User>();

            return result;
        }

        public User GetUser(int userID)
        {
            User result = null;

            UserDataAccess uda = new UserDataAccess();

            result = uda.GetUser(userID);

            return result;
        }

        public User SaveUser(User userInfo)
        {
            User result = null;

            UserDataAccess uda = new UserDataAccess();

            result = uda.SaveUser(userInfo);

            return result;
        }

        public bool DeleteUser(int userID)
        {

            UserDataAccess uda = new UserDataAccess();

            return uda.DeleteUser(userID);

        }
    }
}
