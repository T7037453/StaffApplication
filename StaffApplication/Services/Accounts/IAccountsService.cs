using Microsoft.AspNetCore.Mvc;
using StaffApplication.Models;

namespace StaffApplication.Services.Accounts
{
    public interface IAccountsService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync();

        Task<AccountDto> GetAccountAsync(string id);

        Task<AccountsCreationViewModel> CreateAccountAsync(AccountsCreationViewModel account);

        Task<AccountDto> DeleteAccountAsync(string id);

        Task<AccountsCreationViewModel> EditAccountAsync(AccountsCreationViewModel account, string id);

    }
}
