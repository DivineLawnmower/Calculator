using Calculator.Services.Validation;
using Calculator.Services.Validation.Classes;
using CalculatorTests.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTests
{
    public class ExpressionValidatorTests
    {
        private List<IExpressionValidationRule> GetRules(ExpressionValidator validator)
        {
            var fieldInfo = typeof(ExpressionValidator).GetField("rules", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<IExpressionValidationRule>)fieldInfo.GetValue(validator);
        }

        [Theory]
        [InlineData("test expression")]
        public void Validate_EmptyRules_ReturnsSuccess(string expression)
        {
            var validator = new ExpressionValidator();

            var result = validator.Validate(expression);

            Assert.True(result.Success);
        }

        [Fact]
        public void AddRule_RuleAddedSuccessfully()
        {
            var validator = new ExpressionValidator();

            var rule = new MockExpressionValidationRule(); 

            validator.AddRule(rule);

            var rules = GetRules(validator);

            Assert.Contains(rule, rules);
        }

        [Fact]
        public void RemoveRule_RuleRemovedSuccessfully()
        {
            var validator = new ExpressionValidator();

            var rule = new MockExpressionValidationRule();

            validator.AddRule(rule);

            validator.RemoveRule(rule);

            var rules = GetRules(validator);

            Assert.DoesNotContain(rule, rules);
        }

        [Fact]
        public void RemoveRules_AllRulesRemovedSuccessfully()
        {
            var validator = new ExpressionValidator();
            var rule1 = new MockExpressionValidationRule(); 
            var rule2 = new MockExpressionValidationRule(); 
            validator.AddRule(rule1);
            validator.AddRule(rule2);

            validator.RemoveRules();

            var rules = GetRules(validator);

            Assert.Empty(rules);
        }

        [Fact]
        public void Validate_WithFailingRule_ReturnsValidationFailure()
        {
            var validator = new ExpressionValidator();
            var failingRule = new ContainsInvalidCharacters(); 
            validator.AddRule(failingRule);

            var result = validator.Validate("test expression");

            Assert.False(result.Success);
        }
        [Theory]
        [InlineData("2+2")]
        public void Validate_WithFailingRule_ReturnsValidatioSuccess(string expression)
        {
            var validator = new ExpressionValidator();
            var failingRule = new ContainsInvalidCharacters();
            validator.AddRule(failingRule);

            var result = validator.Validate(expression);

            Assert.True(result.Success);
        }
    }

}
