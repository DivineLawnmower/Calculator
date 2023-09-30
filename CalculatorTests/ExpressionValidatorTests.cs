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

        [Fact]
        public void Validate_EmptyRules_ReturnsSuccess()
        {
            var validator = new ExpressionValidator();

            var result = validator.Validate("test expression");

            Assert.True(result.Success);
        }

        [Fact]
        public void AddRule_RuleAddedSuccessfully()
        {
            // Arrange
            var validator = new ExpressionValidator();
            var rule = new MockExpressionValidationRule(); // Replace with a mock rule implementation.

            // Act
            validator.AddRule(rule);

            // Assert
            var rules = GetRules(validator);

            // Assert
            Assert.Contains(rule, rules);
        }

        [Fact]
        public void RemoveRule_RuleRemovedSuccessfully()
        {
            // Arrange
            var validator = new ExpressionValidator();
            var rule = new MockExpressionValidationRule(); // Replace with a mock rule implementation.
            validator.AddRule(rule);

            // Act
            validator.RemoveRule(rule);

            // Assert
            var rules = GetRules(validator);

            // Assert
            Assert.DoesNotContain(rule, rules);
        }

        [Fact]
        public void RemoveRules_AllRulesRemovedSuccessfully()
        {
            // Arrange
            var validator = new ExpressionValidator();
            var rule1 = new MockExpressionValidationRule(); // Replace with a mock rule implementation.
            var rule2 = new MockExpressionValidationRule(); // Replace with a mock rule implementation.
            validator.AddRule(rule1);
            validator.AddRule(rule2);

            // Act
            validator.RemoveRules();

            // Assert
            var rules = GetRules(validator);

            // Assert
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

    }

}
