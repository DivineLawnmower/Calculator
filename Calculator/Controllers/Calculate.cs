using Calculator.Decorators;
using Calculator.Models;
using Calculator.Services.Calculator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Calculate : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICalculatorService _calculatorService;
        public Calculate(IHttpContextAccessor httpContextAccessor, ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
            _contextAccessor = httpContextAccessor;
        }


        [HttpPost]
        [LimitRequest(MaxRequests = 2, TimeWindow = 5)]
        public IActionResult CalculateExpression([FromBody] ExpressionModel expression)
        {
            try
            {
                if (expression == null || string.IsNullOrWhiteSpace(expression.Expression))
                {
                    return BadRequest("Invalid Expression");
                }
                string toCalculate = expression.Expression;
                var result = _calculatorService.Calculate(toCalculate);
                if (result.Success)
                {
                    return Ok(result.Result);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
