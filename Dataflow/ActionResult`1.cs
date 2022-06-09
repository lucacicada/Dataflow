namespace Dataflow;

/// <summary>
///     A struct representing the result data from an action.
/// </summary>
/// <typeparam name="TInput">
///     The type of input that this <see cref="ActionResult{TInput}"/> operates on
/// </typeparam>
public readonly record struct ActionResult<TInput>
{
    /// <summary> Gets the input data. </summary>
    public TInput Input { get; }

    /// <summary> Gets the exception data, if any. </summary>
    public Exception? Exception { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ActionResult{TInput}"/> struct with the specified data.
    /// </summary>
    /// <param name="input"> The result input. </param>
    public ActionResult(TInput input)
    {
        Input = input;
        Exception = null;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ActionResult{TInput}"/> struct with the specified data.
    /// </summary>
    /// <param name="input"> The result input. </param>
    /// <param name="exception"> The result exception. </param>
    public ActionResult(TInput input, Exception exception!!)
    {
        Input = input;
        Exception = exception;
    }

    /// <inheritdoc />
    public void Deconstruct(out TInput input, out Exception? exception) => (input, exception) = (Input, Exception);
}
