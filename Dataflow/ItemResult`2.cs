namespace Dataflow;

/// <summary>
///     A struct representing the resulting input and value from an operation.
/// </summary>
/// <typeparam name="TInput">
///     The type of input that this <see cref="ItemResult{TInput, TValue}"/> operates on
/// </typeparam>
/// <typeparam name="TValue">
///     The type of output value that this <see cref="ItemResult{TInput, TValue}"/> operates on
/// </typeparam>
public readonly record struct ItemResult<TInput, TValue>
{
    /// <summary> Gets the input data. </summary>
    public TInput Input { get; }

    /// <summary> Gets the value data. </summary>
    public TValue Value { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ItemResult{TInput, TValue}"/> struct with the specified data.
    /// </summary>
    /// <param name="input"> The input item. </param>
    /// <param name="value"> The result value. </param>
    public ItemResult(TInput input, TValue value)
    {
        Input = input;
        Value = value;
    }

    /// <inheritdoc />
    public void Deconstruct(out TInput input, out TValue value) => (input, value) = (Input, Value);
}
