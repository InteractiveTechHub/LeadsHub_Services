

using PhoneNumbers;

namespace LeadsHub.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatPhoneNumber(this string phoneNumberToFormat)
        {
            if (string.IsNullOrWhiteSpace(phoneNumberToFormat)) return string.Empty;

            var phoneUtil = PhoneNumberUtil.GetInstance();
            string internacionalNumber = string.Empty;

            try
            {
                PhoneNumber phoneNumber = phoneUtil.Parse(phoneNumberToFormat, null);
                internacionalNumber = phoneUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL); 
                //var e164 = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164); // +5511912345678
            }
            catch (NumberParseException ex)
            {
                //TODO: log here?? Maybe
                Console.WriteLine($"Erro ao parsear número: {ex.Message}");
            }

            return internacionalNumber;
        }

        public static string RemovePhoneFormat(this string phoneNumberToFormat)
        {
            if (string.IsNullOrWhiteSpace(phoneNumberToFormat)) return string.Empty;

            var phoneUtil = PhoneNumberUtil.GetInstance();
            string numberWithoutFormat = string.Empty;

            try
            {
                PhoneNumber phoneNumber = phoneUtil.Parse(phoneNumberToFormat, null);
                numberWithoutFormat = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164);

                numberWithoutFormat = numberWithoutFormat.TrimStart('+');
            }
            catch (NumberParseException ex)
            {
                //TODO: log here?? Maybe
                Console.WriteLine($"Erro ao parsear número: {ex.Message}");
            }

            return numberWithoutFormat;
        }
    }
}
