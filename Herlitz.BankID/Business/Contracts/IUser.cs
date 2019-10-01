namespace Herlitz.BankID
{
    public interface IUser
    {
        string GivenName { get; set; }
        string Name { get; set; }
        string PersonalNumber { get; set; }
        string Surname { get; set; }
    }
}