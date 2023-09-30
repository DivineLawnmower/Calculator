using Calculator.Services.Validation.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTests.Setup
{
    public class MockExpressionValidationRule : IExpressionValidationRule
    {

        public ValidationResponse Validate(string expression)
        {
            return new ValidationResponse { };
        }
    }
}
