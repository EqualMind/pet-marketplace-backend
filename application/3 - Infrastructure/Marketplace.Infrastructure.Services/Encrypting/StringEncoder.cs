using Marketplace.ApplicationCore.Contracts.Encrypting;

namespace Marketplace.Infrastructure.Services.Encrypting;

public sealed class StringEncoder : IStringEncoder
{
    public bool Compare(string value, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(value, hash);

    public string Hash(string value) => BCrypt.Net.BCrypt.EnhancedHashPassword(value);
}