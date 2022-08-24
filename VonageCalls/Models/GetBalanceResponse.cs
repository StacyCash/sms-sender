namespace VonageCalls.Models;

public struct GetBalanceResponse
{
    public decimal Value { get; set; }
    public bool AutoReload { get; set; }
}