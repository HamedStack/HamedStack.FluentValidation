// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using FluentValidation;

namespace HamedStack.FluentValidation;

/// <summary>
/// Provides a set of extension methods to augment FluentValidation capabilities.
/// </summary>
public static class FluentValidationExtensions
{
    /// <summary>
    /// Adds a duplicate check validation rule to the property.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TValue">The type of the property being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="duplicateCheckFunc">The delegate function to perform the duplicate check.</param>
    /// <returns>The rule builder with the added validation rule.</returns>
    public static IRuleBuilderOptions<T, TValue> DuplicateCheck<T, TValue>(
        this IRuleBuilder<T, TValue> ruleBuilder,
        Func<TValue, bool> duplicateCheckFunc)
    {
        return ruleBuilder.SetValidator(new DuplicateCheckValidator<T, TValue>(duplicateCheckFunc));
    }
}