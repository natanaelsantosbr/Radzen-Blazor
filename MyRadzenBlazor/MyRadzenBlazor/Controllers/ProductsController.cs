using Microsoft.AspNetCore.Mvc;
using MyRadzenBlazor.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductAppService _productAppService;

    public ProductsController(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var retorno = _productAppService.GetProducts();

        return Ok(retorno);
    }
}
