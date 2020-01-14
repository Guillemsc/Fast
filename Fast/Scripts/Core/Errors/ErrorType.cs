using System;


namespace Fast
{
    public enum FastErrorType
    {
        UNDEFINED,

        EMPTY_USERNAME,
        EMPTY_EMAIL,
        EMPTY_PASSWORD,
        EMAIL_ALREADY_IN_USE,
        INVALID_EMAIL,
        WEAK_PASSWORD,
        PASSWORD_CONFIRMATION_MISMATCH,
        PASSWORD_TOO_SHORT,
        USERNAME_TOO_SHORT,
        USERNAME_TOO_LONG,
        ACCOUNT_ALREADY_EXISTS,
        WRONG_CREDENTIALS,
        NETWORK_ERROR,
    }
}
