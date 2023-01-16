using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApplication.Models;
using StaffApplication.Services.Accounts;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;
using System.Xml.Linq;

namespace StaffApplication.Controllers
{
    public class AccountsController : Controller
    {

        private readonly ILogger _logger;
        private readonly IAccountsService _accountsService;
        private readonly IHttpClientFactory _clientFactory;

        public AccountsController(ILogger<AccountsController> logger, 
                                  IAccountsService accountsService)
        {
            _logger = logger;
            _accountsService = accountsService;
            
        }

        // GET: AccountsController
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            IEnumerable<AccountDto> accounts = null;
            try
            {
                accounts = await _accountsService.GetAccountsAsync();

            }
            catch
            {
                _logger.LogWarning("Exception occured using the Account Service");

                accounts = Array.Empty<AccountDto>();

            }
            return View(accounts.ToList());
        }

        // GET: AccountsController/Details/5
        public async Task<IActionResult> Details(string id)

        {
            if (id == null)
            {
                return BadRequest();
            }

            var account = new AccountDto();
            var ViewModel = new AccountsDetailsViewModel();

            try
            {
                account = await _accountsService.GetAccountAsync(id);
                ViewModel.accounts = account;
            }

            catch
            {
                _logger.LogWarning("Exception occured using the Accounts Service");
                ViewModel.accounts = account;
            }

            return View(ViewModel);
        }

        // GET: AccountsController/Create
        [Authorize (Roles ="Management")]
        public ActionResult Create()
        {
            var ViewModel = new AccountsCreationViewModel();
            return View(ViewModel);
        }

        // POST: AccountsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(AccountsCreationViewModel account)
        {
            try
            {
                account = await _accountsService.CreateAccountAsync(account);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Accounts Service");

            }
            return RedirectToAction("Index");

        }

        // GET: AccountsController/Edit/5
        public ActionResult Edit()
        {
            var ViewModel = new AccountsCreationViewModel();
            return View(ViewModel);
        }

        // POST: AccountsController/Edit/5
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountsCreationViewModel account, string id)
        {
            try
            {
                account = await _accountsService.EditAccountAsync(account, id);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Accounts Service");

            }
            return RedirectToAction("Index");
        }

        // GET: AccountsController/Delete/5
        public ActionResult Delete(AccountDto account)
        {
            var ViewModel = new AccountDto();
            ViewModel.user_id = account.user_id;
            
            return View(ViewModel);
        }

        // POST: AccountsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var account = new AccountDto();
            try
            {
                account = await _accountsService.DeleteAccountAsync(id);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Accounts Service");

            }
            return RedirectToAction("Index");
        }
    }
}
