namespace Dataflow;

using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

/// <summary>
///     Provides a dataflow block that delegates its execution to a provided <see cref="IPropagatorBlock{TInput, TOutput}"/>.
/// </summary>
/// <typeparam name="TInput"> 
///     The type of data that the <see cref="IPropagatorBlock{TInput, TOutput}"/> operates on. 
/// </typeparam>
[DebuggerDisplay("{target,nq}")]
[DebuggerTypeProxy(typeof(DelegatePropagatorBlock<>.DebugView))]
public sealed class DelegatePropagatorBlock<TInput> : IPropagatorBlock<TInput, TInput>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ITargetBlock<TInput> target;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ISourceBlock<TInput> source;

    /// <inheritdoc />
    public Task Completion { get; }

    /// <summary> Initializes a new instance of the <see cref="DelegatePropagatorBlock{TInput}" /> class. </summary>
    /// <param name="target"> The <see cref="ITargetBlock{TInput}"/> to delegate the execution to. </param>
    /// <param name="source"> The <see cref="ISourceBlock{TInput}"/> to delegate the execution to. </param>
    /// <param name="completion"> The completion <see cref="Task"/>. </param>
    public DelegatePropagatorBlock(ITargetBlock<TInput> target!!, ISourceBlock<TInput> source!!, Task completion!!)
    {
        this.target = target;
        this.source = source;
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
    TInput? ISourceBlock<TInput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TInput> target, out bool messageConsumed)
       => source.ConsumeMessage(messageHeader, target, out messageConsumed);

    /// <inheritdoc />
    void ISourceBlock<TInput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TInput> target)
       => source.ReleaseReservation(messageHeader, target);

    /// <inheritdoc />
    bool ISourceBlock<TInput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TInput> target)
       => source.ReserveMessage(messageHeader, target);

    /// <inheritdoc />
    IDisposable ISourceBlock<TInput>.LinkTo(ITargetBlock<TInput> target, DataflowLinkOptions linkOptions)
       => source.LinkTo(target, linkOptions);

    /// <inheritdoc />
    public override string ToString()
        => $"PropagatorBlock Id={Completion.Id}";

    /// <summary> The debug view. </summary>
    private sealed class DebugView
    {
        /// <summary> The target block. </summary>
        public ITargetBlock<TInput> TargetBlock { get; }
        /// <summary> The source block. </summary>
        public ISourceBlock<TInput> SourceBlock { get; }

        /// <summary> Creates a new debug view. </summary>
        public DebugView(DelegatePropagatorBlock<TInput> targetBlock)
        {
            TargetBlock = targetBlock.target;
            SourceBlock = targetBlock.source;
        }
    }
}
