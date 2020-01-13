using System;

namespace Fast.GoogleFirebase
{
    public enum FirebaseErrorType
    {
        UNDEFINED,

        EMAIL_ALREADY_IN_USE,
        ACCOUNT_ALREADY_EXISTS,
        INVALID_EMAIL,
        WRONG_CREDENTIALS,
        WEAK_PASSWORD,
        NETWORK_ERROR,
    }

    public class FirebaseExceptionToFirebaseError
    {

#if USING_FIREBASE

        public static FirebaseErrorType Get(Firebase.FirebaseException exception)
        {
            FirebaseErrorType ret = FirebaseErrorType.UNDEFINED;

            if (exception != null)
            {
                Firebase.Auth.AuthError error_code = (Firebase.Auth.AuthError)exception.ErrorCode;

                switch (error_code)
                {
                    case Firebase.Auth.AuthError.EmailAlreadyInUse:
                        {
                            ret = FirebaseErrorType.EMAIL_ALREADY_IN_USE;
                            break;
                        }

                    case Firebase.Auth.AuthError.AccountExistsWithDifferentCredentials:
                        {
                            ret = FirebaseErrorType.ACCOUNT_ALREADY_EXISTS;
                            break;
                        }

                    case Firebase.Auth.AuthError.InvalidEmail:
                        {
                            ret = FirebaseErrorType.INVALID_EMAIL;
                            break;
                        }

                    case Firebase.Auth.AuthError.WrongPassword:
                    case Firebase.Auth.AuthError.InvalidCredential:
                        {
                            ret = FirebaseErrorType.WRONG_CREDENTIALS;
                            break;
                        }

                    case Firebase.Auth.AuthError.WeakPassword:
                        {
                            ret = FirebaseErrorType.WEAK_PASSWORD;
                            break;
                        }

                    case Firebase.Auth.AuthError.NetworkRequestFailed:
                    case Firebase.Auth.AuthError.WebInternalError:
                        {
                            ret = FirebaseErrorType.NETWORK_ERROR;
                            break;
                        }

                    default:
                        {
                            ret = ret = FirebaseErrorType.UNDEFINED;
                            break;
                        }
                }
            }

            return ret;
        }

#endif

    }
}
