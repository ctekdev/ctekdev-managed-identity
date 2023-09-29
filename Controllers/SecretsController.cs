using CtekDev_ManagedIdentity.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CtekDev_ManagedIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly IKeyVaultService _keyVaultService;

        public SecretsController(IKeyVaultService keyVaultService)
        {
                _keyVaultService = keyVaultService;
        }

        [HttpGet(Name = "GetSecret")]
        public async Task<IActionResult> Get(string secretKey)
        {
            var response = await _keyVaultService.GetKeyVaultValue(secretKey);

            return Ok(response);
        }
    }
}