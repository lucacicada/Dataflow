namespace Dataflow;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

/// <summary>
///     Represents a dataflow block that is a target for data providing a counter for the processed items.
/// </summary>
/// <typeparam name="TInput"> 
///     The type of data that the <see cref="ITargetBlock{TInput}"/> operates on. 
/// </typeparam>
public interface ICountTargetBlock<in TInput> : ITargetBlock<TInput>
{
    /// <summary>
    ///     Gets the number of input begin processed by this block.
    /// </summary>
    int Count { get; }
}

/// <summary>
///     Provides a dataflow block that delegates its execution to a provided <see cref="ITargetBlock{TInput}"/> and completes itself.
/// </summary>
/// <typeparam name="TInput"> 
///     The type of data that the <see cref="ITargetBlock{TInput}"/> operates on. 
/// </typeparam>
[DebuggerDisplay("{target,nq}")]
[DebuggerTypeProxy(typeof(AutoCompleteDelegateTargetBlock<>.DebugView))]
public sealed class AutoCompleteDelegateTargetBlock<TInput> : ICountTargetBlock<TInput>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private volatile int count;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ITargetBlock<TInput> target;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ISourceBlock<TInput> source;

    /// <summary>
    ///     Gets the number of input begin processed by this block.
    /// </summary>
    public int Count => count;

    /// <inheritdoc />
    public Task Completion { get; }

    /// <summary> Initializes a new instance of the <see cref="AutoCompleteDelegateTargetBlock{TInput}" /> class. </summary>
    /// <param name="target"> The <see cref="ITargetBlock{TInput}"/> to delegate the execution to. </param>
    /// <param name="source"> The <see cref="ISourceBlock{TInput}"/> to delegate the execution to. </param>
    /// <param name="completion"> The completion <see cref="Task"/>. </param>
    public AutoCompleteDelegateTargetBlock(ITargetBlock<TInput> target!!, ISourceBlock<TInput> source!!, Task completion!!)
    {
        this.target = target;
        this.source = source;
        Completion = completion;

        // link end to this instance
        _ = source.LinkTo(this);
    }

    /// <inheritdoc />
    void IDataflowBlock.Complete() => target.Complete();

    /// <inheritdoc />
    void IDataflowBlock.Fault(Exception exception) { }

    /// <inheritdoc />
    DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept)
    {
        // drop all the messages from the source
        if (source == this.source)
        {
            if (Interlocked.Decrement(ref count) <= 0)
            {
                target.Complete();
            }

            return DataflowMessageStatus.Accepted;
        }

        _ = Interlocked.Increment(ref count);

        return target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
    }

    /// <inheritdoc />
    public override string ToString()
        => $"TargetBlock Id={Completion.Id}";

    /// <summary> The debug view. </summary>
    private sealed class DebugView
    {
        private readonly AutoCompleteDelegateTargetBlock<TInput> _targetBlock;

        /// <summary> The number of input begin processed. </summary>
        public int Count => _targetBlock.count;
        /// <summary> The target block. </summary>
        public ITargetBlock<TInput> TargetBlock { get; }
        /// <summary> The source block. </summary>
        public ISourceBlock<TInput> SourceBlock { get; }

        /// <summary> Creates a new debug view. </summary>
        public DebugView(AutoCompleteDelegateTargetBlock<TInput> targetBlock)
        {
            _targetBlock = targetBlock;
            TargetBlock = targetBlock.target;
            SourceBlock = targetBlock.source;
        }
    }
}
