namespace core.Models;

public class AccountDetail
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public bool PremiumStatus { get; set; }
}