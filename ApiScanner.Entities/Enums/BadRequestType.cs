namespace ApiScanner.Entities.Enums
{
    public enum BadRequestType
    {
        None = 0,
        OtherError = 1,
        PasswordMissmatch = 2,
        UserNotExist = 3,
        EmptyUsername = 4,
        EmptyPassword = 5,
        EmptyEmail = 6,
        InvalidEmail = 7,
        EmailNotConfirmed = 8,
        SignInFailed = 9,
        UserNotFound = 10,
        UserOrPassIncorrect = 11,
        FileNotExist = 12,
        FileAlreadyExist = 13,
    }
}
