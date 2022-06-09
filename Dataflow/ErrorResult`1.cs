namespace Dataflow;

/// <summary>
///     A struct representing the resulting error from an operation.
/// </summary>
/// <typeparam name="TInput">
///     The type of input that this <see cref="ErrorResult{TInput}"/> operates on
/// </typeparam>
public readonly record struct ErrorResult<TInput>
{
    /// <summary> Gets the input data.</summary>
    public TInput Input { get; }

    /// <summary> Gets the exception data, if any.</summary>
    public Exception Exception { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorResult{TInput}"/> struct with the specified data.
    /// </summary>
    /// <param name="input"> The result input. </param>
    /// <param name="exception"> The result exception. </param>
    public ErrorResult(TInput input, Exception exception!!)
    {
        Input = input;
        Exception = exception;
    }

    /// <inheritdoc />
    public void Deconstruct(out TInput input, out Exception exception) => (input, exception) = (Input, Exception);
}
