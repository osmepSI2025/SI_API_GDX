using Microsoft.AspNetCore.Mvc;

namespace SME_API_GDX.Controllers
{
    [ApiController]
    [Route("api/SYS-GDX")]
    public class GDXController : ControllerBase
    {
        private readonly MOrganizationJuristicPersonService _organizationJuristicPersonService;
        public GDXController(MOrganizationJuristicPersonService organizationJuristicPersonService)
        {
            _organizationJuristicPersonService=organizationJuristicPersonService;
        }
        [HttpPost("JuristicPerson/batch")]
        public async Task<IActionResult> batchgetdimenson(searchMOrganizationJuristicPersonModels models)
        {
            await _organizationJuristicPersonService.BatchEndOfDay_MOrganizationJuristicPerson(models);
            return Ok();
        }

        [HttpPost("JuristicPerson")]      
        public async Task<IActionResult> GetAllAsyncSearch_JuristicPerson(searchMOrganizationJuristicPersonModels xmodel)
        {
            var response = await _organizationJuristicPersonService.GetAllAsyncSearch_JuristicPerson(xmodel);

            // Serialize using Newtonsoft.Json to preserve property names
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);

            return Content(json, "application/json");
        }

   
    }
}
