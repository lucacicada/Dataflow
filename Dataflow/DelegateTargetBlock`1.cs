namespace Dataflow;

using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

/// <summary>
///     Provides a dataflow block that delegates its execution to a provided <see cref="ITargetBlock{TInput}"/>.
/// </summary>
/// <typeparam name="TInput"> 
///     The type of data that the <see cref="ITargetBlock{TInput}"/> operates on. 
/// </typeparam>
[DebuggerDisplay("{target,nq}")]
[DebuggerTypeProxy(typeof(DelegateTargetBlock<>.DebugView))]
public sealed class DelegateTargetBlock<TInput> : ITargetBlock<TInput>
{
    /// <summary> The target block to delegate to. </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ITargetBlock<TInput> target;

    /// <inheritdoc />
    public Task Completion { get; }

    /// <summary> Initializes a new instance of the <see cref="DelegateTargetBlock{TInput}" /> class. </summary>
    /// <param name="target"> The <see cref="ITargetBlock{TInput}"/> to delegate the execution to. </param>
    /// <param name="completion"> The completion <see cref="Task"/>. </param>
    public DelegateTargetBlock(ITargetBlock<TInput> target!!, Task completion!!)
    {
        this.target = target;
        Completion = completion;
    }

    /// <inheritdoc />
    void IDataflowBlock.Complete() => target.Complete();

    /// <inheritdoc />
    void IDataflowBlock.Fault(Exception exception) { }

    /// <inheritdoc />
    DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept)
       => target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);

    /// <inheritdoc />
    public override string ToString()
        => $"TargetBlock Id={Completion.Id}";

    /// <summary> The debug view. </summary>
    private sealed class DebugView
    {
        /// <summary> The target block. </summary>
        public ITargetBlock<TInput> TargetBlock { get; }

        /// <summary> Creates a new debug view. </summary>
        public DebugView(DelegateTargetBlock<TInput> targetBlock)
        {
            TargetBlock = targetBlock.target;
        }
    }
}
