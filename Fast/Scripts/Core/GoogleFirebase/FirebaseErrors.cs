using System;

namespace Fast.GoogleFirebase
{
    public class FirebaseExceptionToFastError
    {

#if USING_FIREBASE_AUTH || USING_FIREBASE_ANALYTICS

        public static FastErrorType GetError(Firebase.FirebaseException exception)
        {
            FastErrorType ret = FastErrorType.UNDEFINED;

            if (exception != null)
            {
                Firebase.Auth.AuthError error_code = (Firebase.Auth.AuthError)exception.ErrorCode;

                switch (error_code)
                {
                    case Firebase.Auth.AuthError.EmailAlreadyInUse:
                        {
                            ret = FastErrorType.EMAIL_ALREADY_IN_USE;
                            break;
                        }

                    case Firebase.Auth.AuthError.AccountExistsWithDifferentCredentials:
                        {
                            ret = FastErrorType.ACCOUNT_ALREADY_EXISTS;
                            break;
                        }

                    case Firebase.Auth.AuthError.InvalidEmail:
                        {
                            ret = FastErrorType.INVALID_EMAIL;
                            break;
                        }

                    case Firebase.Auth.AuthError.WrongPassword:
                    case Firebase.Auth.AuthError.InvalidCredential:
                        {
                            ret = FastErrorType.WRONG_CREDENTIALS;
                            break;
                        }

                    case Firebase.Auth.AuthError.WeakPassword:
                        {
                            ret = FastErrorType.WEAK_PASSWORD;
                            break;
                        }

                    case Firebase.Auth.AuthError.NetworkRequestFailed:
                    case Firebase.Auth.AuthError.WebInternalError:
                        {
                            ret = FastErrorType.NETWORK_ERROR;
                            break;
                        }

                    default:
                        {
                            ret = ret = FastErrorType.UNDEFINED;
                            break;
                        }
                }
            }

            return ret;
        }

#endif

    }
}
