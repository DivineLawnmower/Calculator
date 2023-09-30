using Calculator.Services.Calculator;
using Calculator.Services.Evaluation;
using Calculator.Services.Validation.Classes;
using Calculator.Services.Validation;
using Xunit;
using Moq;
using System.ComponentModel.DataAnnotations;
using static Calculator.Services.Calculator.Calculations;
using NuGet.Frameworks;
using System.Linq.Expressions;

namespace CalculatorTests
{

    public class CalculatorTests
    {
        public static IEnumerable<object[]> CalculatorDataSuccess =>
            new List<object[]>
            {
                new object[] { GetBasicOperations(), "4+5*2", new List<string>() { "4","5", "2","*","+" },  14 },
                new object[] { GetBasicOperations(), "4+5/2", new List<string>() { "4", "5", "2", "/", "+" }, 6.5 },
                new object[] { GetBasicOperations(), "4+5/2-1", new List<string>() { "4", "5", "2","/", "+", "1", "-" }, 5.5 },
            };


        public static IEnumerable<object[]> CalculatorDataFailure =>
            new List<object[]>
            {
                new object[] { GetBasicOperations(), "4+5*", new List<string>() { "4","5","*"},  14 },
                new object[] { GetBasicOperations(), "4+5/t+", new List<string>() { "4", "5", "t", "/", "+" }, 6.5 },
                new object[] { GetBasicOperations(), "4+5/0", new List<string>() { "4", "5", "0","/", "+" }, 5.5 },
            };

        public static Dictionary<string, IOperation> GetBasicOperations()
        {
            return new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() }
            };
        }

        public static Dictionary<string, IOperation> GetAdvancedOperations()
        {
            var operations = GetBasicOperations();

            return operations;
        }

        public static IList<IExpressionValidationRule> SetupValidationRules()
        {
            return new List<IExpressionValidationRule>()
            {
                new ContainsInvalidCharacters()
            };
        }

        public static ExpressionValidator SetupValidator()
        {
            ExpressionValidator validator = new ExpressionValidator();
            validator.SetRules(SetupValidationRules());
            return validator;
        }
        public static CalculatorService SetupCalcService()
        {
            ExpressionValidator validator = SetupValidator();

            Evaluator evaluator = new Evaluator();
            evaluator.SetOperations(GetBasicOperations());

            return new CalculatorService(validator, evaluator);
        }


        [Theory]
        [MemberData(nameof(CalculatorDataSuccess))]
        public void EvaluateExpression_ShouldReturnCorrectResult(
        Dictionary<string, IOperation> operations,
        string expression, List<string> shuntYard, double expectedResult)
        {
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.True(result.Success);
            Assert.Equal(expectedResult, result.Result);
        }


        [Theory]
        [MemberData(nameof(CalculatorDataFailure))]
        public void EvaluateExpression_ShouldThrowArgumentException_InvalidExpression(
            Dictionary<string, IOperation> operations,
            string expression, List<string> shuntYard, double expectedResult)
        {
        
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.False(result.Success);    
            Assert.NotEqual(expectedResult, result.Result);
        }

        [Fact]
        public void Calculate_ValidExpression_ReturnsResult()
        {
            // Arrange
            var validatorMock = new Mock<IExpressionValidator>();
            validatorMock.Setup(v => v.Validate(It.IsAny<string>())).Returns(new ValidationResponse());

            var evaluatorMock = new Mock<IEvaluator>();
            evaluatorMock.Setup(e => e.Evaluate(It.IsAny<string>())).Returns(new EvaluatorResponse(new List<string>()
                {
                    "4",
                    "5",
                    "2",
                    "*",
                    "+"
                })
                );

            var calculatorService = new CalculatorService(validatorMock.Object, evaluatorMock.Object);

            // Act
            var result = calculatorService.Calculate("4+5*2");

            // Assert
            Assert.Equal(14, result.Result);
            validatorMock.Verify(v => v.Validate("4+5*2"), Times.Once);
            evaluatorMock.Verify(e => e.Evaluate("4+5*2"), Times.Once);
        }

        [Fact]
        public void Calculate_InvalidExpression_ReturnsValidationMessage()
        {
            var validatorMock = new Mock<IExpressionValidator>();
            validatorMock.Setup(v => v.Validate(It.IsAny<string>())).Returns(new ValidationResponse("Invalid"));

            var evaluatorMock = new Mock<IEvaluator>();

            var calculatorService = new CalculatorService(validatorMock.Object, evaluatorMock.Object);

            var result = calculatorService.Calculate("invalid expression");

            Assert.Equal(result.Result, 0);
            Assert.False(result.Success);
            Assert.NotNull(result.Message);
            validatorMock.Verify(v => v.Validate("invalid expression"), Times.Once);
            evaluatorMock.Verify(e => e.Evaluate(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("2 + 3", 5)]
        [InlineData("51+49", 100)]
        public void EvaluateExpression_ShouldReturnCorrectResult_Addition(string expression, double expectedResult)
        { 
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.True(result.Success);
            Assert.Equal(expectedResult, result.Result);
        }

        [Theory]
        [InlineData("3 - 2", 1)]
        [InlineData("51-49", 2)]
        public void EvaluateExpression_ShouldReturnCorrectResult_Subtraction(string expression, double expectedResult)
        {
         
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.True(result.Success);
            Assert.Equal(expectedResult, result.Result);
        }

        [Theory]
        [InlineData("8 * 2", 16)]
        [InlineData("20*5", 100)]
        public void EvaluateExpression_ShouldReturnCorrectResult_Multiplication(string expression, double expectedResult)
        {
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.True(result.Success);
            Assert.Equal(expectedResult, result.Result);
        }


        [Theory]
        [InlineData("8 / 2", 4)]
        [InlineData("20/5", 4)]
        public void EvaluateExpression_ShouldReturnCorrectResult_Division(string expression, double expectedResult)
        {
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.True(result.Success);
            Assert.Equal(expectedResult, result.Result);
        }

        [Theory]
        [InlineData("8 / 0")]
        [InlineData("20/0")]
        public void EvaluateExpression_ShouldThrowException_DivisionByZero(string expression)
        {
            CalculatorService calculator = SetupCalcService();
            CalculationResponse result = calculator.Calculate(expression);

            Assert.False(result.Success);
        }


    }
}

