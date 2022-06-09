namespace Dataflow;

using System.Threading.Tasks.Dataflow;

/// <remarks>
///     It is not possible to match the <see langword="null" /> type, therefore all null values are discarded.
///     Uncaught exceptions are discarded.
/// </remarks>
public sealed class ObjectBatchTargetBlockBuilder
{
    private readonly BufferBlock<object?> bufferBlock;
    private readonly List<IDataflowBlock> blocks = new();

    public ObjectBatchTargetBlockBuilder()
    {
        bufferBlock = new BufferBlock<object?>();
    }
    public ObjectBatchTargetBlockBuilder(DataflowBlockOptions dataflowBlockOptions!!)
    {
        bufferBlock = new BufferBlock<object?>(dataflowBlockOptions);
    }

    public ObjectBatchTargetBlockBuilder Batch<TInput>(int batchSize, Action<TInput[]> action!!)
    {
        Batch(action, new BatchBlock<TInput>(batchSize));

        return this;
    }
    public ObjectBatchTargetBlockBuilder Batch<TInput>(Action<TInput[]> action!!, BatchBlock<TInput> batchBlock!!)
    {
        BatchInternal(new ActionBlock<TInput[]>(items =>
        {
            try
            {
                action(items);
            }
            catch
            {
                //
            }
        }), batchBlock);

        return this;
    }
    public ObjectBatchTargetBlockBuilder Batch<TInput>(Action<TInput[]> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!, BatchBlock<TInput> batchBlock!!)
    {
        BatchInternal(new ActionBlock<TInput[]>(items =>
        {
            try
            {
                action(items);
            }
            catch
            {
                //
            }
        }, dataflowBlockOptions), batchBlock);

        return this;
    }

    public ObjectBatchTargetBlockBuilder Batch<TInput>(int batchSize, Func<TInput[], Task> action!!)
    {
        Batch(action, new BatchBlock<TInput>(batchSize));

        return this;
    }
    public ObjectBatchTargetBlockBuilder Batch<TInput>(Func<TInput[], Task> action!!, BatchBlock<TInput> batchBlock!!)
    {
        BatchInternal(new ActionBlock<TInput[]>(async items =>
        {
            try
            {
                await action(items);
            }
            catch
            {
                //
            }
        }), batchBlock);

        return this;
    }
    public ObjectBatchTargetBlockBuilder Batch<TInput>(Func<TInput[], Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!, BatchBlock<TInput> batchBlock!!)
    {
        BatchInternal(new ActionBlock<TInput[]>(async items =>
        {
            try
            {
                await action(items);
            }
            catch
            {
                //
            }
        }, dataflowBlockOptions), batchBlock);

        return this;
    }

    private void BatchInternal<TInput>(ActionBlock<TInput[]> actionBlock, BatchBlock<TInput> batchBlock)
    {
        blocks.Add(batchBlock);

        // propagate the completion down the pipe
        var propagateComplete = new DataflowLinkOptions
        {
            PropagateCompletion = true
        };

        // link the blocks
        _ = batchBlock.LinkTo(actionBlock, propagateComplete);

        // transform the object
        var transformBlock = new TransformBlock<object?, TInput>(_ => (TInput)_!); // TODO: this can be removed and the action could perform the cast
        _ = transformBlock.LinkTo(batchBlock, propagateComplete);

        _ = bufferBlock.LinkTo(transformBlock, propagateComplete, p => p is TInput);
    }

    public ITargetBlock<object?> Build()
    {
        _ = bufferBlock.LinkTo(DataflowBlock.NullTarget<object?>());

        List<Task> completions = new()
        {
            bufferBlock.Completion
        };

        foreach (IDataflowBlock dataflowBlock in blocks)
        {
            completions.Add(dataflowBlock.Completion);
        }

        return new DelegateTargetBlock<object?>(bufferBlock, Task.WhenAll(completions));
    }
}
