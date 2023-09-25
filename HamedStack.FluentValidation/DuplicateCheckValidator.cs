using FluentValidation;
using FluentValidation.Validators;

namespace HamedStack.FluentValidation;

/// <summary>
/// A custom property validator that checks for duplicate values using a specified function.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
/// <typeparam name="TValue">The type of the value being validated.</typeparam>
public class DuplicateCheckValidator<T, TValue> : PropertyValidator<T, TValue>
{
    private readonly Func<TValue, bool> _duplicateCheckFunc;
    private readonly bool _ignoreSelf;
    private readonly Func<T, TValue>? _selfValueSelector;
    private readonly string _errorMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateCheckValidator{T, TValue}"/> class.
    /// </summary>
    /// <param name="duplicateCheckFunc">A function that determines whether a value is a duplicate.</param>
    /// <param name="ignoreSelf">A flag indicating whether to ignore self when checking for duplicates.</param>
    /// <param name="selfValueSelector">A function to select the value from the instance being validated.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    public DuplicateCheckValidator(
        Func<TValue, bool> duplicateCheckFunc,
        bool ignoreSelf = false,
        Func<T, TValue>? selfValueSelector = null,
        string? errorMessage = null)
    {
        _duplicateCheckFunc = duplicateCheckFunc ?? throw new ArgumentNullException(nameof(duplicateCheckFunc));
        _ignoreSelf = ignoreSelf;
        _selfValueSelector = selfValueSelector;
        _errorMessage = errorMessage ?? "{PropertyName} already exists.";
    }

    /// <inheritdoc/>
    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return _errorMessage;
    }

    /// <inheritdoc/>
    public override string Name => "DuplicateCheckValidator";

    /// <inheritdoc/>
    public override bool IsValid(ValidationContext<T> context, TValue value)
    {
        if (!_ignoreSelf || _selfValueSelector == null) return !_duplicateCheckFunc(value);

        var selfValue = _selfValueSelector(context.InstanceToValidate);
        if (EqualityComparer<TValue>.Default.Equals(selfValue, value))
        {
            return true;
        }

        return !_duplicateCheckFunc(value);
    }
}