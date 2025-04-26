

using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads
{
    public class WhatsAppPayLoad
    {
        public List<PayloadEntry> Entry { get; set; } = new();

        public string Object { get; set; } = string.Empty;
    }

    public sealed class PayloadEntry
    {
        public IEnumerable<PayLoadChange> Changes { get; set; } = [];

        public string Id { get; set; } = string.Empty;

    }

    public sealed class PayLoadChange
    {
        public string Field { get; set; } = string.Empty;

        public PayLoadValue Value { get; set; } = new();
    }

    public sealed class PayLoadValue
    {
        public string Messaging_Product { get; set; } = string.Empty;

        public PayLoadMetadata Metadata { get; set; } = new();

        public IEnumerable<PayLoadContact> Contacts { get; set; } = [];

        public IEnumerable<PayLoadMessage> Messages { get; set; } = [];
    }

    public sealed class PayLoadMetadata
    {
        [JsonPropertyName("Display_Phone_Number")]
        public string DisplayPhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("Phone_Number_Id")]
        public string PhoneNumberId { get; set; } = string.Empty;
    }

    public sealed class PayLoadContact
    {
        public PayLoadProfile Profile { get; set; } = new();

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }

    public sealed class PayLoadMessage
    {
        public string Id { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public PayLoadText Text { get; set; } = new();

        public PayloadReaction Reaction { get; set; } = new();

        public PayloadImage Image { get; set; } = new();

        public PayloadImage Audio { get; set; } = new();

        public PayloadImage Video { get; set; } = new();

        public PayloadImage Document { get; set; } = new();

        public string TimeStamp { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public IEnumerable<PayLoadContactToReceive> Contacts { get; set; } = [];
    }

    public sealed class PayLoadProfile
    {
        public string Name { get; set; } = string.Empty;
    }

    public sealed class PayLoadText
    {
        /// <summary>
        /// This is the message that user writes and send
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }

    public sealed class PayloadReaction
    {
        public string Emoji { get; set; } = string.Empty;

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = string.Empty;
    }

    public class PayloadImage
    {
        public string Caption { get; set; } = string.Empty;

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;

        public string Sha256 { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
    }

    public sealed class PayLoadContactToReceive
    {
        public IEnumerable<PayLoadAddress> Addresses { get; set; } = [];

        public string BirthDay { get; set; } = string.Empty;

        public IEnumerable<PayLoadEmail> Emails { get; set; } = [];

        public PayLoadName Name { get; set; } = new();

        public PayLoadOrg Org { get; set; } = new();

        public IEnumerable<PayLoadPhone> Phones { get; set; } = [];

        public IEnumerable<PayLoadUrl> Urls { get; set; } = [];
    }

    public sealed class PayLoadAddress
    {
        /// <summary>
        /// Street number and name
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State code
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Zip code
        /// </summary>
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Country name
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Country code
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Address type
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }

    public class PayLoadEmail
    {
        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Email type
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }

    public sealed class PayLoadName
    {
        /// <summary>
        /// Full Name
        /// </summary>
        [JsonPropertyName("formatted_name")]
        public string FormattedName { get; set; } = string.Empty;

        /// <summary>
        /// First Name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Middle name
        /// </summary>
        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; } = string.Empty;

        /// <summary>
        /// Preffix
        /// </summary>
        public string Preffix { get; set; } = string.Empty;
    }

    public sealed class PayLoadOrg
    {
        /// <summary>
        /// Company or organization name
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Department name
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Job title
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }

    public sealed class PayLoadPhone
    {
        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Whatsapp user Id
        /// </summary>
        [JsonPropertyName("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }

    public class PayLoadUrl
    {
        /// <summary>
        /// Url of the website
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Website type
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
