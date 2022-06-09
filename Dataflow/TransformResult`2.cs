namespace Dataflow;

/// <summary>
///     A struct representing the resulting data from a transformation.
/// </summary>
public readonly record struct TransformResult<TInput, TOutput>
{
    /// <summary> Gets the input data, if any.</summary>
    public TInput? Input { get; }

    /// <summary> Gets the output data, if any.</summary>
    public TOutput? Output { get; }

    /// <summary> Gets the exception data, if any.</summary>
    public Exception? Exception { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TransformResult{TInput, TOutput}"/> struct with the specified data.
    /// </summary>
    /// <param name="output"> The result output. </param>
    public TransformResult(TOutput output)
    {
        Input = default;
        Output = output;
        Exception = null;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TransformResult{TInput, TOutput}"/> struct with the specified data.
    /// </summary>
    /// <param name="input"> The result input. </param>
    /// <param name="exception"> The result exception. </param>
    public TransformResult(TInput input, Exception exception!!)
    {
        Input = input;
        Output = default;
        Exception = exception;
    }

    /// <inheritdoc />
    public void Deconstruct(out TInput? input, out TOutput? output, out Exception? exception) => (input, output, exception) = (Input, Output, Exception);
}
