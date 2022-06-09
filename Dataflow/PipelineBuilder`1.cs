namespace Dataflow;

using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

public class WhenBuilder<TInput>
{
    internal IPropagatorBlock<TInput, TransformResult<TInput, ItemResult<TInput, bool>>>? EnterBlock;
    internal IPropagatorBlock<TInput, ActionResult<TInput>>? ActionBlock;
    internal IPropagatorBlock<TInput, ActionResult<TInput>>? ExitBlock;

    /// <summary>
    ///     When false, skip this branch.
    /// </summary>
    public WhenBuilder<TInput> UseConditionalQueue(Func<TInput, bool> action!!)
        => AssingAndReturn(ref EnterBlock, Dataflow.TransformWithItem(action));

    /// <summary>
    ///     When false, skip this branch.
    /// </summary>
    public WhenBuilder<TInput> UseConditionalQueue(Func<TInput, bool> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref EnterBlock, Dataflow.TransformWithItem(action, dataflowBlockOptions));

    /// <summary>
    ///     When false, skip this branch.
    /// </summary>
    public WhenBuilder<TInput> UseConditionalQueue(Func<TInput, Task<bool>> action!!)
        => AssingAndReturn(ref EnterBlock, Dataflow.TransformWithItem(action));

    /// <summary>
    ///     When false, skip this branch.
    /// </summary>
    public WhenBuilder<TInput> UseConditionalQueue(Func<TInput, Task<bool>> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref EnterBlock, Dataflow.TransformWithItem(action, dataflowBlockOptions));

    public WhenBuilder<TInput> UseAction(Action<TInput> action!!)
        => AssingAndReturn(ref ActionBlock, Dataflow.Propagator(action));

    public WhenBuilder<TInput> UseAction(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref ActionBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    public WhenBuilder<TInput> UseAction(Func<TInput, Task> action!!)
        => AssingAndReturn(ref ActionBlock, Dataflow.Propagator(action));

    public WhenBuilder<TInput> UseAction(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref ActionBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    public WhenBuilder<TInput> UseCompletion(Action<TInput> action!!)
        => AssingAndReturn(ref ExitBlock, Dataflow.Propagator(action));

    public WhenBuilder<TInput> UseCompletion(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref ExitBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    public WhenBuilder<TInput> UseCompletion(Func<TInput, Task> action!!)
        => AssingAndReturn(ref ExitBlock, Dataflow.Propagator(action));

    public WhenBuilder<TInput> UseCompletion(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref ExitBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    //
    private WhenBuilder<TInput> AssingAndReturn<T>(ref T reference, T value)
    {
        reference = value;
        return this;
    }
}

public class PipelineBuilder<TInput>
{
    private readonly List<WhenData<TInput>> list = new();
    private IPropagatorBlock<TInput, ActionResult<TInput>>? _queueBlock;
    private IPropagatorBlock<TInput, ActionResult<TInput>>? _actionBlock;
    private IPropagatorBlock<ErrorResult<TInput>, TInput>? _exceptionHandlerBlock;
    private ActionBlockBuilder<TInput>? _completionBlockBuilder;

    private void Reset()
    {
        list.Clear();
        _queueBlock = null;
        _actionBlock = null;
        _exceptionHandlerBlock = null;
        _completionBlockBuilder = null;
    }

    /// <summary>
    ///     Conditionally creates a branch in the request pipeline that is rejoined to the main pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseWhen(Predicate<TInput> predicate!!, Action<WhenBuilder<TInput>> configure!!)
    {
        var b = new WhenBuilder<TInput>();
        configure(b);

        if (b.ActionBlock is null)
        {
            throw new InvalidOperationException($"Call {nameof(WhenBuilder<TInput>.UseAction)}.");
        }

        if (b.ExitBlock is null && b.EnterBlock is null)
        {
            list.Add(new WhenData<TInput>(predicate, b.ActionBlock));
        }
        else
        {
            if (b.EnterBlock is null)
            {
                throw new InvalidOperationException($"Call {nameof(WhenBuilder<TInput>.UseConditionalQueue)}.");
            }

            if (b.ExitBlock is null)
            {
                throw new InvalidOperationException($"Call {nameof(WhenBuilder<TInput>.UseCompletion)}.");
            }

            list.Add(new WhenData<TInput>(predicate, b.ActionBlock, b.EnterBlock, b.ExitBlock));
        }

        return this;
    }

    /// <summary>
    ///     Set an action that is invoked at the beginning in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseQueue(Action<TInput> action!!)
        => AssingAndReturn(ref _queueBlock, Dataflow.Propagator(action));

    /// <summary>
    ///     Set an action that is invoked at the beginning in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseQueue(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _queueBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked at the beginning in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseQueue(Func<TInput, Task> action!!)
        => AssingAndReturn(ref _queueBlock, Dataflow.Propagator(action));

    /// <summary>
    ///     Set an action that is invoked at the beginning in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseQueue(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _queueBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked after the queue and any conditional branch in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseAction(Action<TInput> action!!)
        => AssingAndReturn(ref _actionBlock, Dataflow.Propagator(action));

    /// <summary>
    ///     Set an action that is invoked after the queue and any conditional branch in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseAction(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _actionBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked after the queue and any conditional branch in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseAction(Func<TInput, Task> action!!)
        => AssingAndReturn(ref _actionBlock, Dataflow.Propagator(action));

    /// <summary>
    ///     Set an action that is invoked after the queue and any conditional branch in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseAction(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _actionBlock, Dataflow.Propagator(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked when an error occour in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseExceptionHandler(Action<TInput, Exception> action!!)
        => AssingAndReturn(ref _exceptionHandlerBlock, Dataflow.ErrorPropagatorWithoutException(action));

    /// <summary>
    ///     Set an action that is invoked when an error occour in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseExceptionHandler(Action<TInput, Exception> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _exceptionHandlerBlock, Dataflow.ErrorPropagatorWithoutException(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked when an error occour in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseExceptionHandler(Func<TInput, Exception, Task> action!!)
        => AssingAndReturn(ref _exceptionHandlerBlock, Dataflow.ErrorPropagatorWithoutException(action));

    /// <summary>
    ///     Set an action that is invoked when an error occour in the request pipeline.
    /// </summary>
    public PipelineBuilder<TInput> UseExceptionHandler(Func<TInput, Exception, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _exceptionHandlerBlock, Dataflow.ErrorPropagatorWithoutException(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked at the end in the request pipeline.
    /// </summary>
    /// <remarks>
    ///     This action will be always invoked, even if an error occour.
    /// </remarks>
    public PipelineBuilder<TInput> UseCompletion(Action<TInput> action!!)
        => AssingAndReturn(ref _completionBlockBuilder, new ActionBlockBuilder<TInput>(action));

    /// <summary>
    ///     Set an action that is invoked at the end in the request pipeline.
    /// </summary>
    /// <remarks>
    ///     This action will be always invoked, even if an error occour.
    /// </remarks>
    public PipelineBuilder<TInput> UseCompletion(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _completionBlockBuilder, new ActionBlockBuilder<TInput>(action, dataflowBlockOptions));

    /// <summary>
    ///     Set an action that is invoked at the end in the request pipeline.
    /// </summary>
    /// <remarks>
    ///     This action will be always invoked, even if an error occour.
    /// </remarks>
    public PipelineBuilder<TInput> UseCompletion(Func<TInput, Task> action!!)
        => AssingAndReturn(ref _completionBlockBuilder, new ActionBlockBuilder<TInput>(action));

    /// <summary>
    ///     Set an action that is invoked at the end in the request pipeline.
    /// </summary>
    /// <remarks>
    ///     This action will be always invoked, even if an error occour.
    /// </remarks>
    public PipelineBuilder<TInput> UseCompletion(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
        => AssingAndReturn(ref _completionBlockBuilder, new ActionBlockBuilder<TInput>(action, dataflowBlockOptions));

    //
    private PipelineBuilder<TInput> AssingAndReturn<T>(ref T reference, T value)
    {
        reference = value;
        return this;
    }

    /// <summary>
    ///     Build a target block.
    /// </summary>
    public ITargetBlock<TInput> BuildTarget()
    {
        ITargetBlock<TInput> queue = _queueBlock ?? (ITargetBlock<TInput>)new BufferBlock<TInput>();
        ITargetBlock<TInput> end = _completionBlockBuilder is null ? DataflowBlock.NullTarget<TInput>() : _completionBlockBuilder.CreateTargetBlockSupressException();

        var completionTask = LinkBlocks(
            queue,
            end,
            _actionBlock,
            _exceptionHandlerBlock,
            list
        );

        Reset();

        return new DelegateTargetBlock<TInput>(queue, completionTask);
    }

    /// <summary>
    ///     Build a target block that completes itself.
    /// </summary>
    public ITargetBlock<TInput> BuildAutoCompleteTarget()
    {
        var q = _queueBlock ?? (ITargetBlock<TInput>)new BufferBlock<TInput>();
        IPropagatorBlock<TInput, TInput> e = _completionBlockBuilder is null ? new TransformBlock<TInput, TInput>(_ => _) : _completionBlockBuilder.CreatePropagatorBlockSupressException();

        var completion = LinkBlocks(
            q,
            e,
            _actionBlock,
            _exceptionHandlerBlock,
            list
        );

        Reset();

        return new AutoCompleteDelegateTargetBlock<TInput>(q, e, completion);
    }

    /// <summary>
    ///     Build a propagator block.
    /// </summary>
    public IPropagatorBlock<TInput, TInput> BuildPropagator()
    {
        var q = _queueBlock ?? (ITargetBlock<TInput>)new BufferBlock<TInput>();
        IPropagatorBlock<TInput, TInput> e = _completionBlockBuilder is null ? new TransformBlock<TInput, TInput>(_ => _) : _completionBlockBuilder.CreatePropagatorBlockSupressException();

        var completion = LinkBlocks(
            q,
            e,
            _actionBlock,
            _exceptionHandlerBlock,
            list
        );

        Reset();

        return new DelegatePropagatorBlock<TInput>(q, e, completion);
    }

    /// <param name="queue">
    ///     The initial block.
    /// </param>
    /// <param name="action">
    ///     The action block, it will run on action.
    /// </param>
    /// <param name="error">
    ///     The errro block, it will run on error.
    /// </param>
    /// <param name="completion">
    ///     The last block, it will run after action or error.
    /// </param>
    /// <param name="list"></param>
    private static Task LinkBlocks(
        ITargetBlock<TInput> queue!!,
        ITargetBlock<TInput> completion!!,
        ITargetBlock<TInput>? action,
        IPropagatorBlock<ErrorResult<TInput>, TInput>? error,
        IList<WhenData<TInput>>? list)
    {
        // ignore the list
        list ??= new List<WhenData<TInput>>();

        // ingore the queue
        // queue ??= new BufferBlock<TInput>();

        // ignore the action
        action ??= new BufferBlock<TInput>();

        // ignore the error
        error ??= new TransformBlock<ErrorResult<TInput>, TInput>(error => error.Input);

        DataflowLinkOptions linkOptions = new() { PropagateCompletion = true };

        _ = error.LinkTo(completion);

        if (action is ISourceBlock<ActionResult<TInput>> actionResult)
        {
            _ = actionResult.LinkToError(error);
            _ = actionResult.LinkToNext(completion);
        }
        else if (action is BufferBlock<TInput> bufferBlock)
        {
            _ = bufferBlock.LinkTo(completion);
        }
        else if (action is ISourceBlock<TInput> targetBlock)
        {
            _ = targetBlock.LinkTo(completion);
        }

        // link queue to error
        bool queueLinkedToError = false;
        if (queue is ISourceBlock<ActionResult<TInput>> actionResultSource0)
        {
            _ = actionResultSource0.LinkToError(error);
            queueLinkedToError = true;
        }

        foreach (var tru in list)
        {
            if (tru is null)
            {
                throw new ArgumentException();
            }

            ITargetBlock<TInput> next;

            if (tru.ExitBlock is null)
            {
                Debug.Assert(tru.EnterBlock == null);
                Debug.Assert(tru.ExitBlock == null);

                next = tru.ActionBlock;

                _ = tru.ActionBlock.LinkToError(error);
                _ = tru.ActionBlock.LinkToNext(action);
            }
            else
            {
                Debug.Assert(tru.EnterBlock != null);
                Debug.Assert(tru.ExitBlock != null);

                next = tru.EnterBlock;

                _ = tru.ExitBlock.LinkToError(error);
                _ = tru.ExitBlock.LinkToNext(action);

                _ = tru.ActionBlock.LinkToError(error);
                _ = tru.ActionBlock.LinkToNext(tru.ExitBlock, linkOptions);

                _ = tru.EnterBlock.LinkToError(error);
                _ = tru.EnterBlock.LinkToTransformNext(tru.ActionBlock, linkOptions, data => data.Input, data => data.Value);
                _ = tru.EnterBlock.LinkToTransformNext(action, data => data.Input, data => !data.Value);
            }

            if (queue is ISourceBlock<ActionResult<TInput>> actionResultSource1)
            {
                if (tru.Predicate is null)
                {
                    _ = actionResultSource1.LinkToNext(next, linkOptions);
                }
                else
                {
                    _ = actionResultSource1.LinkToNext(next, linkOptions, tru.Predicate);
                }
            }
            else if (queue is ISourceBlock<TInput> sourceTInput1)
            {
                if (tru.Predicate is null)
                {
                    _ = sourceTInput1.LinkTo(next, linkOptions);
                }
                else
                {
                    _ = sourceTInput1.LinkTo(next, linkOptions, tru.Predicate);
                }
            }
            else
            {
                throw new InvalidOperationException($"invalid {nameof(queue)} block type");
            }
        }

        bool queueLinkedToEnd = false;
        bool queueLinkedToAction = false;

        if (list.Count == 0)
        {
            // link queue to action
            if (queue is ISourceBlock<ActionResult<TInput>> actionResultSource)
            {
                _ = actionResultSource.LinkToNextDiscardError(action);
                queueLinkedToAction = true;
            }
            else if (queue is ISourceBlock<TInput> sourceTInput)
            {
                _ = sourceTInput.LinkTo(action);
                queueLinkedToAction = true;
            }
            else
            {
                throw new InvalidOperationException($"invalid {nameof(queue)} block type");
            }
        }
        else
        {
            // link queue to end
            if (queue is ISourceBlock<ActionResult<TInput>> actionResultSource)
            {
                _ = actionResultSource.LinkToNextDiscardError(completion);
                queueLinkedToEnd = true;
            }
            else if (queue is ISourceBlock<TInput> sourceTInput)
            {
                _ = sourceTInput.LinkTo(completion);
                queueLinkedToEnd = true;
            }
            else
            {
                throw new InvalidOperationException($"invalid {nameof(queue)} block type");
            }
        }

        // queue will complete readCache and download
        // action is linked to action or both enter/exit blocks and/or action
        var tasks1 = list.SelectMany(p =>
        {
            if (p.EnterBlock != null)
            {
                return new[] { p.EnterBlock.Completion, p.ExitBlock!.Completion };
            }

            return new[] { p.ActionBlock.Completion };
        }).ToList();

        if (queueLinkedToAction)
        {
            tasks1.Add(queue.Completion);
        }

        Dataflow.PropagateCompletionTo(action, tasks1);

        // error is linked to all except end
        Dataflow.PropagateCompletionTo(error, list.SelectMany(p => new[] {
            p.ActionBlock.Completion,
            p.ExitBlock?.Completion,
            p.EnterBlock?.Completion }
        ).Concat(new[] {
            action.Completion,
            queueLinkedToError ? queue.Completion : null,
        }).OfType<Task>());

        // end is linked to both error and action
        if (queueLinkedToEnd) Dataflow.PropagateCompletionTo(completion, error, action, queue);
        else Dataflow.PropagateCompletionTo(completion, error, action);

        List<Task> tasks = new();
        tasks.Add(queue.Completion);
        tasks.Add(action.Completion);
        tasks.Add(error.Completion);
        tasks.Add(completion.Completion);
        foreach (var tru in list)
        {
            tasks.Add(tru.ActionBlock.Completion);
            if (tru.EnterBlock != null) tasks.Add(tru.EnterBlock.Completion);
            if (tru.ExitBlock != null) tasks.Add(tru.ExitBlock.Completion);
        }

        return Task.WhenAll(tasks);
    }

    //
    private sealed class WhenData<T>
    {
        public readonly Predicate<T>? Predicate;
        public IPropagatorBlock<T, TransformResult<T, ItemResult<T, bool>>>? EnterBlock { get; }
        public IPropagatorBlock<T, ActionResult<T>> ActionBlock { get; }
        public IPropagatorBlock<T, ActionResult<T>>? ExitBlock { get; }

        public WhenData(
            Predicate<T> predicate,
            IPropagatorBlock<T, ActionResult<T>> action)
        {
            this.Predicate = predicate;
            this.ActionBlock = action;
        }

        public WhenData(
            Predicate<T> predicate,
            IPropagatorBlock<T, ActionResult<T>> action,
            IPropagatorBlock<T, TransformResult<T, ItemResult<T, bool>>>? enter,
            IPropagatorBlock<T, ActionResult<T>>? exit)
        {
            this.Predicate = predicate;
            this.ActionBlock = action;
            this.EnterBlock = enter;
            this.ExitBlock = exit;
        }
    }
}
