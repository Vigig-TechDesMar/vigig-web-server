namespace Vigig.Common.Constants.Validations;

public static class UserProfileValidation
{
    public static class Email
    {
        public const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // public const string NotMatchedPatternMessage = "Please enter a valid email address.";
        public const string NotMatchedPatternMessage = "Vui lòng đăng nhập địa chỉ email hợp lệ.";
    }

    public static class Password
    {
        public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        // public const string NotMatchedPatternMessage = "Password must contain at least 8 characters, one uppercase letter, one number and one special character.";
        public const string NotMatchedPatternMessage = "Mật khẩu chứa ít nhất 8 kí tự, 1 chữ viết hoa, 1 chữ số và 1 kí tự đặc biệt.";
        // public const string InvalidPasswordMessage = "Invalid password";
        public const string InvalidPasswordMessage = "Mật khẩu không hợp lệ";
    }
}