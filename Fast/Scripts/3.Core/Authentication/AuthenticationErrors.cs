using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Authentication
{
    public class AuthenticationDataToFastError
    {
        public static bool GetLoginError(string email, string password, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            if(!ret && String.IsNullOrEmpty(email))
            {
                error = FastErrorType.EMPTY_EMAIL;

                ret = true;
            }

            if (!ret && String.IsNullOrEmpty(password))
            {
                error = FastErrorType.EMPTY_PASSWORD;

                ret = true;
            }

            return ret;
        }

        public static bool GetRegistrationError(string email, string password, string password_conf, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            ret = GetRegistrationErrorEmail(email, out error);

            if(!ret)
            {
                ret = GetRegistrationErrorPassword(password, password_conf, out error);
            }

            return ret;
        }

        public static bool GetRegistrationError(string username, string email, string password, string password_conf, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            ret = GetRegistrationErrorUsername(username, out error);

            if (!ret)
            {
                ret = GetRegistrationErrorEmail(email, out error);
            }

            if (!ret)
            {
                ret = GetRegistrationErrorPassword(password, password_conf, out error);
            }

            return ret;
        }

        public static bool GetRegistrationErrorUsername(string username, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            if (!ret && String.IsNullOrEmpty(username))
            {
                error = FastErrorType.EMPTY_USERNAME;

                ret = true;
            }

            if (!ret && username.Length < 3)
            {
                error = FastErrorType.USERNAME_TOO_SHORT;

                ret = true;
            }

            if (!ret && username.Length > 16)
            {
                error = FastErrorType.USERNAME_TOO_LONG;

                ret = true;
            }

            return ret;
        }

        public static bool GetRegistrationErrorEmail(string email, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            if (!ret && String.IsNullOrEmpty(email))
            {
                error = FastErrorType.EMPTY_EMAIL;

                ret = true;
            }

            return ret;
        }

        public static bool GetRegistrationErrorPassword(string password, string password_conf, out FastErrorType error)
        {
            bool ret = false;

            error = FastErrorType.UNDEFINED;

            if (!ret && (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password_conf)))
            {
                error = FastErrorType.EMPTY_PASSWORD;

                ret = true;
            }

            if (!ret && password.Length < 5)
            {
                error = FastErrorType.PASSWORD_TOO_SHORT;

                ret = true;
            }

            if (!ret && password_conf != password)
            {
                error = FastErrorType.PASSWORD_CONFIRMATION_MISMATCH;

                ret = true;
            }

            return ret;
        }
    }
}