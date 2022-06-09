namespace Dataflow;

using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

public static partial class Dataflow
{
    /// <summary>
    ///     Propagate the completion to the specific block when all the tasks have completed. 
    /// </summary>
    public static void PropagateCompletionTo(IDataflowBlock block!!, IEnumerable<Task> tasks!!)
    {
        _ = Task.WhenAll(tasks).ContinueWith(_ => block.Complete());
    }

    /// <summary>
    ///     Propagate the completion to the specific block when all the tasks have completed. 
    /// </summary>
    public static void PropagateCompletionTo(IDataflowBlock block!!, params Task[] tasks!!)
    {
        _ = Task.WhenAll(tasks).ContinueWith(_ => block.Complete());
    }

    /// <summary>
    ///     Propagate the completion to the specific block when all the blocks have completed. 
    /// </summary>
    public static void PropagateCompletionTo(IDataflowBlock block!!, IEnumerable<IDataflowBlock> blocks!!)
    {
        _ = Task.WhenAll(blocks.Select(p => p.Completion)).ContinueWith(_ => block.Complete());
    }

    /// <summary>
    ///     Propagate the completion to the specific block when all the blocks have completed. 
    /// </summary>
    public static void PropagateCompletionTo(IDataflowBlock block!!, params IDataflowBlock[] blocks!!)
    {
        _ = Task.WhenAll(blocks.Select(p => p.Completion)).ContinueWith(_ => block.Complete());
    }

    //
    /// <summary> Link a result. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNext<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!)
    {
        var filter = new ResultPropagatorBlock<TOutput>(source, target, null);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNext<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        DataflowLinkOptions linkOptions!!)
    {
        var filter = new ResultPropagatorBlock<TOutput>(source, target, null);
        return source.LinkTo(filter, linkOptions);
    }

    /// <summary> Link a result. </summary>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNext<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new ResultPropagatorBlock<TOutput>(source, target, predicate);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result. </summary>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNext<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        DataflowLinkOptions linkOptions!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new ResultPropagatorBlock<TOutput>(source, target, predicate);
        return source.LinkTo(filter, linkOptions);
    }

    //
    /// <summary> Link a result. </summary>
    /// <remarks> Transform exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToTransformNext<TInput, TOutput>(
        this ISourceBlock<TransformResult<TInput, TOutput>> source!!,
        ITargetBlock<TInput> target!!,
        Func<TOutput, TInput> transform!!)
    {
        var filter = new TransformResultPropagatorBlock<TInput, TOutput>(source, target, transform, null);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result. </summary>
    /// <remarks> Transform exceptions are fatal. </remarks>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToTransformNext<TInput, TOutput>(
        this ISourceBlock<TransformResult<TInput, TOutput>> source!!,
        ITargetBlock<TInput> target!!,
        Func<TOutput, TInput> transform!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new TransformResultPropagatorBlock<TInput, TOutput>(source, target, transform, predicate);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result. </summary>
    /// <remarks> Transform exceptions are fatal. </remarks>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToTransformNext<TInput, TOutput>(
        this ISourceBlock<TransformResult<TInput, TOutput>> source!!,
        ITargetBlock<TInput> target!!,
        DataflowLinkOptions linkOptions!!,
        Func<TOutput, TInput> transform!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new TransformResultPropagatorBlock<TInput, TOutput>(source, target, transform, predicate);
        return source.LinkTo(filter, linkOptions);
    }

    //
    /// <summary> Link an error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<ErrorResult<TOutput>> target!!)
    {
        var filter = new ErrorPropagatorBlock<TOutput>(source, target);
        return source.LinkTo(filter);
    }

    /// <summary> Link an error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<ErrorResult<TOutput>> target!!,
        DataflowLinkOptions linkOptions!!)
    {
        var filter = new ErrorPropagatorBlock<TOutput>(source, target);
        return source.LinkTo(filter, linkOptions);
    }

    //
    /// <summary> Link an error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToError<TInput, TOutput>(
        this ISourceBlock<TransformResult<TInput, TOutput>> source!!,
        ITargetBlock<ErrorResult<TInput>> target!!)
    {
        var filter = new TransformErrorPropagatorBlock<TInput, TOutput>(source, target);
        return source.LinkTo(filter);
    }

    /// <summary> Link an error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToError<TInput, TOutput>(
        this ISourceBlock<TransformResult<TInput, TOutput>> source!!,
        ITargetBlock<ErrorResult<TInput>> target!!,
        DataflowLinkOptions linkOptions!!)
    {
        var filter = new TransformErrorPropagatorBlock<TInput, TOutput>(source, target);
        return source.LinkTo(filter, linkOptions);
    }

    //
    /// <summary> Link a result dropping the error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNextDiscardError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!)
    {
        var filter = new DiscardErrorPropagatorBlock<TOutput>(source, target, null);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result dropping the error. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNextDiscardError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        DataflowLinkOptions linkOptions!!)
    {
        var filter = new DiscardErrorPropagatorBlock<TOutput>(source, target, null);
        return source.LinkTo(filter, linkOptions);
    }

    /// <summary> Link a result dropping the error. </summary>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNextDiscardError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new DiscardErrorPropagatorBlock<TOutput>(source, target, predicate);
        return source.LinkTo(filter);
    }

    /// <summary> Link a result dropping the error. </summary>
    /// <remarks> Predicate exceptions are fatal. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="source"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="target"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="linkOptions"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="predicate"/> is <see langword="null" />. </exception>
    public static IDisposable LinkToNextDiscardError<TOutput>(
        this ISourceBlock<ActionResult<TOutput>> source!!,
        ITargetBlock<TOutput> target!!,
        DataflowLinkOptions linkOptions!!,
        Predicate<TOutput> predicate!!)
    {
        var filter = new DiscardErrorPropagatorBlock<TOutput>(source, target, predicate);
        return source.LinkTo(filter, linkOptions);
    }

    //
    private sealed class DiscardErrorPropagatorBlock<T> : IPropagatorBlock<ActionResult<T>, T>
    {
        /// <summary> The source connected with this filter. </summary>
        private readonly ISourceBlock<ActionResult<T>> _source;
        /// <summary> The target with which this block is associated. </summary>
        private readonly ITargetBlock<T> _target;
        /// <summary>The predicate provided by the user.</summary>
        private readonly Predicate<T>? _userProvidedPredicate;

        internal DiscardErrorPropagatorBlock(ISourceBlock<ActionResult<T>> source, ITargetBlock<T> target, Predicate<T>? predicate)
        {
            Debug.Assert(source != null, "Filtered link requires a source to filter on.");
            Debug.Assert(target != null, "Filtered link requires a target to filter to.");

            this._source = source;
            this._target = target;
            this._userProvidedPredicate = predicate;
        }

        Task IDataflowBlock.Completion => _source.Completion;
        void IDataflowBlock.Complete() => _target.Complete();
        void IDataflowBlock.Fault(Exception exception) => _target.Fault(exception);

        DataflowMessageStatus ITargetBlock<ActionResult<T>>.OfferMessage(DataflowMessageHeader messageHeader, ActionResult<T> messageValue, ISourceBlock<ActionResult<T>>? source, bool consumeToAccept)
        {
            if (!messageHeader.IsValid) throw new ArgumentException("The DataflowMessageHeader instance does not represent a valid message header.", nameof(messageHeader));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var (item, _) = messageValue;

            if ((_userProvidedPredicate is null || _userProvidedPredicate(item)))
            {
                return _target.OfferMessage(messageHeader, item, this, consumeToAccept);
            }
            else return DataflowMessageStatus.Declined;
        }

        //
        T? ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            var (item, _) = _source.ConsumeMessage(messageHeader, this, out messageConsumed);
            return item;
        }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            return _source.ReserveMessage(messageHeader, this);
        }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            _source.ReleaseReservation(messageHeader, this);
        }

        IDisposable ISourceBlock<T>.LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            throw new NotSupportedException("This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member.");
        }
    }

    private sealed class ResultPropagatorBlock<T> : IPropagatorBlock<ActionResult<T>, T>
    {
        /// <summary> The source connected with this filter. </summary>
        private readonly ISourceBlock<ActionResult<T>> _source;
        /// <summary> The target with which this block is associated. </summary>
        private readonly ITargetBlock<T> _target;
        /// <summary>The predicate provided by the user.</summary>
        private readonly Predicate<T>? _userProvidedPredicate;

        internal ResultPropagatorBlock(ISourceBlock<ActionResult<T>> source, ITargetBlock<T> target, Predicate<T>? predicate)
        {
            Debug.Assert(source != null, "Filtered link requires a source to filter on.");
            Debug.Assert(target != null, "Filtered link requires a target to filter to.");

            this._source = source;
            this._target = target;
            this._userProvidedPredicate = predicate;
        }

        Task IDataflowBlock.Completion => _source.Completion;
        void IDataflowBlock.Complete() => _target.Complete();
        void IDataflowBlock.Fault(Exception exception) => _target.Fault(exception);

        DataflowMessageStatus ITargetBlock<ActionResult<T>>.OfferMessage(DataflowMessageHeader messageHeader, ActionResult<T> messageValue, ISourceBlock<ActionResult<T>>? source, bool consumeToAccept)
        {
            if (!messageHeader.IsValid) throw new ArgumentException("The DataflowMessageHeader instance does not represent a valid message header.", nameof(messageHeader));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var (item, ex) = messageValue;

            if (ex is null && (_userProvidedPredicate is null || _userProvidedPredicate(item)))
            {
                return _target.OfferMessage(messageHeader, item, this, consumeToAccept);
            }
            else return DataflowMessageStatus.Declined;
        }

        //
        T? ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            var (item, ex) = _source.ConsumeMessage(messageHeader, this, out messageConsumed);
            Debug.Assert(ex is null, "Only valid messages may be consumed.");
            return item;
        }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            return _source.ReserveMessage(messageHeader, this);
        }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            _source.ReleaseReservation(messageHeader, this);
        }

        IDisposable ISourceBlock<T>.LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            throw new NotSupportedException("This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member.");
        }
    }

    private sealed class ErrorPropagatorBlock<T> : IPropagatorBlock<ActionResult<T>, ErrorResult<T>>
    {
        /// <summary> The source connected with this filter. </summary>
        private readonly ISourceBlock<ActionResult<T>> _source;
        /// <summary> The target with which this block is associated. </summary>
        private readonly ITargetBlock<ErrorResult<T>> _target;

        internal ErrorPropagatorBlock(ISourceBlock<ActionResult<T>> source, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(source != null, "Filtered link requires a source to filter on.");
            Debug.Assert(target != null, "Filtered link requires a target to filter to.");

            this._source = source;
            this._target = target;
        }

        Task IDataflowBlock.Completion => _source.Completion;
        void IDataflowBlock.Complete() => _target.Complete();
        void IDataflowBlock.Fault(Exception exception) => _target.Fault(exception);

        DataflowMessageStatus ITargetBlock<ActionResult<T>>.OfferMessage(DataflowMessageHeader messageHeader, ActionResult<T> messageValue, ISourceBlock<ActionResult<T>>? source, bool consumeToAccept)
        {
            if (!messageHeader.IsValid) throw new ArgumentException("The DataflowMessageHeader instance does not represent a valid message header.", nameof(messageHeader));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var (item, ex) = messageValue;

            if (ex is not null)
            {
                return _target.OfferMessage(messageHeader, new ErrorResult<T>(item, ex), this, consumeToAccept);
            }
            else return DataflowMessageStatus.Declined;
        }

        //
        ErrorResult<T> ISourceBlock<ErrorResult<T>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target, out bool messageConsumed)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            var (item, ex) = _source.ConsumeMessage(messageHeader, this, out messageConsumed);
            Debug.Assert(ex is not null, "Only valid messages may be consumed.");
            return new ErrorResult<T>(item, ex);
        }

        bool ISourceBlock<ErrorResult<T>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            return _source.ReserveMessage(messageHeader, this);
        }

        void ISourceBlock<ErrorResult<T>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            _source.ReleaseReservation(messageHeader, this);
        }

        IDisposable ISourceBlock<ErrorResult<T>>.LinkTo(ITargetBlock<ErrorResult<T>> target, DataflowLinkOptions linkOptions)
        {
            throw new NotSupportedException("This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member.");
        }
    }

    //
    private sealed class TransformResultPropagatorBlock<T, TOutput> : IPropagatorBlock<TransformResult<T, TOutput>, T>
    {
        /// <summary> The source connected with this filter. </summary>
        private readonly ISourceBlock<TransformResult<T, TOutput>> _source;
        /// <summary> The target with which this block is associated. </summary>
        private readonly ITargetBlock<T> _target;
        /// <summary>The transform provided by the user.</summary>
        private readonly Func<TOutput, T> _userProvidedTransform;
        /// <summary>The predicate provided by the user.</summary>
        private readonly Predicate<TOutput>? _userProvidedPredicate;

        internal TransformResultPropagatorBlock(
            ISourceBlock<TransformResult<T, TOutput>> source,
            ITargetBlock<T> target,
            Func<TOutput, T> transform,
            Predicate<TOutput>? predicate)
        {
            Debug.Assert(source != null, "Filtered link requires a source to filter on.");
            Debug.Assert(target != null, "Filtered link requires a target to filter to.");
            Debug.Assert(transform != null, "Filtered link requires a transform to filter to.");

            this._source = source;
            this._target = target;
            this._userProvidedTransform = transform;
            this._userProvidedPredicate = predicate;
        }
        Task IDataflowBlock.Completion => _source.Completion;
        void IDataflowBlock.Complete() => _target.Complete();
        void IDataflowBlock.Fault(Exception exception) => _target.Fault(exception);

        DataflowMessageStatus ITargetBlock<TransformResult<T, TOutput>>.OfferMessage(DataflowMessageHeader messageHeader, TransformResult<T, TOutput> messageValue, ISourceBlock<TransformResult<T, TOutput>>? source, bool consumeToAccept)
        {
            if (!messageHeader.IsValid) throw new ArgumentException("The DataflowMessageHeader instance does not represent a valid message header.", nameof(messageHeader));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var (item, output, ex) = messageValue;

            if (ex is null && (_userProvidedPredicate is null || _userProvidedPredicate(output!)))
            {
                var input = _userProvidedTransform(output!);

                return _target.OfferMessage(messageHeader, input, this, consumeToAccept);
            }
            else return DataflowMessageStatus.Declined;
        }

        //
        T? ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            var (item, _, ex) = _source.ConsumeMessage(messageHeader, this, out messageConsumed);
            Debug.Assert(ex is null, "Only valid messages may be consumed.");
            return item;
        }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            return _source.ReserveMessage(messageHeader, this);
        }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            _source.ReleaseReservation(messageHeader, this);
        }

        IDisposable ISourceBlock<T>.LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            throw new NotSupportedException("This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member.");
        }
    }

    private sealed class TransformErrorPropagatorBlock<T, TOutput> : IPropagatorBlock<TransformResult<T, TOutput>, ErrorResult<T>>
    {
        /// <summary> The source connected with this filter. </summary>
        private readonly ISourceBlock<TransformResult<T, TOutput>> _source;
        /// <summary> The target with which this block is associated. </summary>
        private readonly ITargetBlock<ErrorResult<T>> _target;

        internal TransformErrorPropagatorBlock(ISourceBlock<TransformResult<T, TOutput>> source, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(source != null, "Filtered link requires a source to filter on.");
            Debug.Assert(target != null, "Filtered link requires a target to filter to.");

            this._source = source;
            this._target = target;
        }

        Task IDataflowBlock.Completion => _source.Completion;
        void IDataflowBlock.Complete() => _target.Complete();
        void IDataflowBlock.Fault(Exception exception) => _target.Fault(exception);

        DataflowMessageStatus ITargetBlock<TransformResult<T, TOutput>>.OfferMessage(DataflowMessageHeader messageHeader, TransformResult<T, TOutput> messageValue, ISourceBlock<TransformResult<T, TOutput>>? source, bool consumeToAccept)
        {
            if (!messageHeader.IsValid) throw new ArgumentException("The DataflowMessageHeader instance does not represent a valid message header.", nameof(messageHeader));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var (item, _, ex) = messageValue;

            if (ex is not null)
            {
                return _target.OfferMessage(messageHeader, new ErrorResult<T>(item!, ex), this, consumeToAccept);
            }
            else return DataflowMessageStatus.Declined;
        }

        //
        ErrorResult<T> ISourceBlock<ErrorResult<T>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target, out bool messageConsumed)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            var (item, _, ex) = _source.ConsumeMessage(messageHeader, this, out messageConsumed);
            Debug.Assert(ex is not null, "Only valid messages may be consumed.");
            return new ErrorResult<T>(item!, ex);
        }

        bool ISourceBlock<ErrorResult<T>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            return _source.ReserveMessage(messageHeader, this);
        }

        void ISourceBlock<ErrorResult<T>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<ErrorResult<T>> target)
        {
            Debug.Assert(messageHeader.IsValid, "Only valid messages may be consumed.");
            _source.ReleaseReservation(messageHeader, this);
        }

        IDisposable ISourceBlock<ErrorResult<T>>.LinkTo(ITargetBlock<ErrorResult<T>> target, DataflowLinkOptions linkOptions)
        {
            throw new NotSupportedException("This member is not supported on this dataflow block. The block is intended for a specific purpose that does not utilize this member.");
        }
    }
}

// Error
public static partial class Dataflow
{
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, ActionResult<TInput>> ErrorPropagator<TInput>(Action<TInput, Exception> action!!)
    {
        return new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(error =>
        {
            try
            {
                action(error.Input, error.Exception);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, ActionResult<TInput>> ErrorPropagator<TInput>(Action<TInput, Exception> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(error =>
        {
            try
            {
                action(error.Input, error.Exception);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, ActionResult<TInput>> ErrorPropagator<TInput>(Func<TInput, Exception, Task> action!!)
    {
        return new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, ActionResult<TInput>> ErrorPropagator<TInput>(Func<TInput, Exception, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> The <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, TInput> ErrorPropagatorWithoutException<TInput>(Action<TInput, Exception> action!!)
    {
        return new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            try
            {
                action(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        });
    }

    /// <summary> Creates a propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, TInput> ErrorPropagatorWithoutException<TInput>(Action<TInput, Exception> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            try
            {
                action(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, TInput> ErrorPropagatorWithoutException<TInput>(Func<TInput, Exception, Task> action!!)
    {
        return new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        });
    }

    /// <summary> Creates a propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<ErrorResult<TInput>, TInput> ErrorPropagatorWithoutException<TInput>(Func<TInput, Exception, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a non propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<ErrorResult<TInput>> ErrorTarget<TInput>(Action<TInput, Exception> action!!)
    {
        return new ActionBlock<ErrorResult<TInput>>(error =>
        {
            try
            {
                action(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a non propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<ErrorResult<TInput>> ErrorTarget<TInput>(Action<TInput, Exception> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new ActionBlock<ErrorResult<TInput>>(error =>
        {
            try
            {
                action(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a non propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<ErrorResult<TInput>> ErrorTarget<TInput>(Func<TInput, Exception, Task> action!!)
    {
        return new ActionBlock<ErrorResult<TInput>>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a non propagation error block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<ErrorResult<TInput>> ErrorTarget<TInput>(Func<TInput, Exception, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new ActionBlock<ErrorResult<TInput>>(async error =>
        {
            try
            {
                await action(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }
}

// Propagator
public static partial class Dataflow
{
    /// <summary> Creates a propagation block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="InvalidOperationException"> The <paramref name="delegate"/> is an invalid type. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagateFromDelegateSupressException<TInput>(Delegate @delegate!!, ExecutionDataflowBlockOptions? dataflowBlockOptions) => @delegate switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ? PropagatorSupressException(delegateAction) : PropagatorSupressException(delegateAction, dataflowBlockOptions),
        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ? PropagatorSupressException(delegateTask) : PropagatorSupressException(delegateTask, dataflowBlockOptions),
        _ => throw new InvalidOperationException($"{nameof(@delegate)} is an invalid type."),
    };

    /// <summary> Creates a propagation block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput>(Action<TInput> action!!)
    {
        return new TransformBlock<TInput, TInput>(item =>
        {
            try
            {
                action(item);
            }
            catch
            {
                // TODO: trace
            }

            return item;
        });
    }

    /// <summary> Creates a propagation block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput>(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TInput>(item =>
        {
            try
            {
                action(item);
            }
            catch
            {
                // TODO: trace
            }

            return item;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a propagation block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput>(Func<TInput, Task> action!!)
    {
        return new TransformBlock<TInput, TInput>(async item =>
        {
            try
            {
                await action(item).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return item;
        });
    }

    /// <summary> Creates a propagation block. </summary>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput>(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TInput>(async item =>
        {
            try
            {
                await action(item).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return item;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> The <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput, T>(Action<T> action!!) where T : TInput
    {
        return new TransformBlock<TInput, TInput>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }
            }
            catch
            {
                // TODO: trace
            }

            return input;
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput, T>(Action<T> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new TransformBlock<TInput, TInput>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput, T>(Func<T, Task> action!!) where T : TInput
    {
        return new TransformBlock<TInput, TInput>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }
            }
            catch
            {
                // TODO: trace
            }

            return input;
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TInput> PropagatorSupressException<TInput, T>(Func<T, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new TransformBlock<TInput, TInput>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput>(Action<TInput> action!!)
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                action(input);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput>(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                action(input);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput>(Func<TInput, Task> action!!)
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                await action(input).ConfigureAwait(false);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput>(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                await action(input).ConfigureAwait(false);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> The <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput, T>(Action<T> action!!) where T : TInput
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput, T>(Action<T> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput, T>(Func<T, Task> action!!) where T : TInput
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        });
    }

    /// <summary> Creates a block that can propagate an exception. </summary>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, ActionResult<TInput>> Propagator<TInput, T>(Func<T, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions);
    }
}

// Target
public static partial class Dataflow
{
    /// <summary> Creates a dataflow block that invokes a provided <see cref="Delegate"/> delegate without throwing an exception. </summary>
    /// <exception cref="InvalidOperationException"> The <paramref name="delegate"/> is an invalid type. </exception>
    public static ITargetBlock<TInput> TargetFromDelegate<TInput>(Delegate @delegate!!, ExecutionDataflowBlockOptions? dataflowBlockOptions) => @delegate switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ? Target(delegateAction) : Target(delegateAction, dataflowBlockOptions),
        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ? Target(delegateTask) : Target(delegateTask, dataflowBlockOptions),
        _ => throw new InvalidOperationException($"{nameof(@delegate)} is an invalid type."),
    };

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput>(Action<TInput> action!!)
    {
        return new ActionBlock<TInput>(input =>
        {
            try
            {
                action(input);
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <param name="dataflowBlockOptions"> The options with which to configure the <see cref="ActionBlock{TInput}"/>. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput>(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new ActionBlock<TInput>(input =>
        {
            try
            {
                action(input);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput>(Func<TInput, Task> action!!)
    {
        return new ActionBlock<TInput>(async input =>
        {
            try
            {
                await action(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <param name="dataflowBlockOptions"> The options with which to configure the <see cref="ActionBlock{TInput}"/>. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput>(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new ActionBlock<TInput>(async input =>
        {
            try
            {
                await action(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput, T>(Action<T> action!!) where T : TInput
    {
        return new ActionBlock<TInput>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <param name="dataflowBlockOptions"> The options with which to configure the <see cref="ActionBlock{TInput}"/>. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput, T>(Action<T> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new ActionBlock<TInput>(input =>
        {
            try
            {
                if (input is T inputType)
                {
                    action(inputType);
                }
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput, T>(Func<T, Task> action!!) where T : TInput
    {
        return new ActionBlock<TInput>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }
            }
            catch
            {
                // TODO: trace
            }
        });
    }

    /// <summary> Creates a dataflow block that invokes a provided <see cref="System.Action{T}"/> delegate without throwing an exception. </summary>
    /// <param name="action"> The action to invoke. </param>
    /// <param name="dataflowBlockOptions"> The options with which to configure the <see cref="ActionBlock{TInput}"/>. </param>
    /// <remarks> Uncaught exceptions are discarded. </remarks>
    /// <exception cref="ArgumentNullException"> <paramref name="action"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static ITargetBlock<TInput> Target<TInput, T>(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!) where T : TInput
    {
        return new ActionBlock<TInput>(async input =>
        {
            try
            {
                if (input is T inputType)
                {
                    await action(inputType).ConfigureAwait(false);
                }
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions);
    }
}

// Transform
public static partial class Dataflow
{
    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, TOutput>> Transform<TInput, TOutput>(Func<TInput, TOutput> transform!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, TOutput>>(item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(transform(item));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, TOutput>> Transform<TInput, TOutput>(Func<TInput, TOutput> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, TOutput>>(item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(transform(item));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, TOutput>> Transform<TInput, TOutput>(Func<TInput, Task<TOutput>> transform!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, TOutput>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(await transform(item).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, TOutput>> Transform<TInput, TOutput>(Func<TInput, Task<TOutput>> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, TOutput>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(await transform(item).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>> TransformWithItem<TInput, TOutput>(Func<TInput, TOutput> transform!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, transform(item)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>> TransformWithItem<TInput, TOutput>(Func<TInput, TOutput> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, transform(item)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }, dataflowBlockOptions);
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>> TransformWithItem<TInput, TOutput>(Func<TInput, Task<TOutput>> transform!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, await transform(item).ConfigureAwait(false)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        });
    }

    /// <exception cref="ArgumentNullException"> <paramref name="transform"/> is <see langword="null" />. </exception>
    /// <exception cref="ArgumentNullException"> <paramref name="dataflowBlockOptions"/> is <see langword="null" />. </exception>
    public static IPropagatorBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>> TransformWithItem<TInput, TOutput>(Func<TInput, Task<TOutput>> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        return new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, await transform(item).ConfigureAwait(false)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }, dataflowBlockOptions);
    }
}

public sealed class ErrorBlockBuilder<TInput>
{
    private readonly Delegate action;
    private readonly ExecutionDataflowBlockOptions? dataflowBlockOptions;

    public ErrorBlockBuilder(Action<TInput, Exception> action!!)
    {
        this.action = action;
    }
    public ErrorBlockBuilder(Action<TInput, Exception> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.action = action;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }
    public ErrorBlockBuilder(Func<TInput, Exception, Task> action!!)
    {
        this.action = action;
    }
    public ErrorBlockBuilder(Func<TInput, Exception, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.action = action;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }

    public ActionBlock<ErrorResult<TInput>> CreateTargetBlock() => action switch
    {
        Action<TInput, Exception> delegateAction => dataflowBlockOptions is null ?
        new ActionBlock<ErrorResult<TInput>>(error => delegateAction(error.Input, error.Exception)) :
        new ActionBlock<ErrorResult<TInput>>(error => delegateAction(error.Input, error.Exception), dataflowBlockOptions),

        Func<TInput, Exception, Task> delegateTask => dataflowBlockOptions is null ?
        new ActionBlock<ErrorResult<TInput>>(async error => await delegateTask(error.Input, error.Exception).ConfigureAwait(false)) :
        new ActionBlock<ErrorResult<TInput>>(async error => await delegateTask(error.Input, error.Exception).ConfigureAwait(false), dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public ActionBlock<ErrorResult<TInput>> CreateTargetBlockSupressException() => action switch
    {
        Action<TInput, Exception> delegateAction => dataflowBlockOptions is null ?
        new ActionBlock<ErrorResult<TInput>>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }
        }) :
        new ActionBlock<ErrorResult<TInput>>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions),

        Func<TInput, Exception, Task> delegateTask => dataflowBlockOptions is null ?
        new ActionBlock<ErrorResult<TInput>>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }) :
        new ActionBlock<ErrorResult<TInput>>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<ErrorResult<TInput>, TInput> CreatePropagatorBlock() => action switch
    {
        Action<TInput, Exception> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            delegateAction(error.Input, error.Exception);

            return error.Input;
        }) :
        new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            delegateAction(error.Input, error.Exception);

            return error.Input;
        }, dataflowBlockOptions),

        Func<TInput, Exception, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            await delegateTask(error.Input, error.Exception).ConfigureAwait(false);

            return error.Input;
        }) :
        new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            await delegateTask(error.Input, error.Exception).ConfigureAwait(false);

            return error.Input;
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<ErrorResult<TInput>, TInput> CreatePropagatorBlockSuppressException() => action switch
    {
        Action<TInput, Exception> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }) :
        new TransformBlock<ErrorResult<TInput>, TInput>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }, dataflowBlockOptions),

        Func<TInput, Exception, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }) :
        new TransformBlock<ErrorResult<TInput>, TInput>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return error.Input;
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<ErrorResult<TInput>, ActionResult<TInput>> CreateResultPropagatorBlock() => action switch
    {
        Action<TInput, Exception> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }) :
        new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(error =>
        {
            try
            {
                delegateAction(error.Input, error.Exception);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }, dataflowBlockOptions),

        Func<TInput, Exception, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }) :
        new TransformBlock<ErrorResult<TInput>, ActionResult<TInput>>(async error =>
        {
            try
            {
                await delegateTask(error.Input, error.Exception).ConfigureAwait(false);

                return new ActionResult<TInput>(error.Input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(error.Input, ex);
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };
}

public sealed class ActionBlockBuilder<TInput>
{
    private readonly Delegate action;
    private readonly ExecutionDataflowBlockOptions? dataflowBlockOptions;

    public ActionBlockBuilder(Action<TInput> action!!)
    {
        this.action = action;
    }
    public ActionBlockBuilder(Action<TInput> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.action = action;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }
    public ActionBlockBuilder(Func<TInput, Task> action!!)
    {
        this.action = action;
    }
    public ActionBlockBuilder(Func<TInput, Task> action!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.action = action;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }

    public ActionBlock<TInput> CreateTargetBlock() => action switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ?
        new ActionBlock<TInput>(delegateAction) :
        new ActionBlock<TInput>(delegateAction, dataflowBlockOptions),

        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ?
        new ActionBlock<TInput>(delegateTask) :
        new ActionBlock<TInput>(delegateTask, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public ActionBlock<TInput> CreateTargetBlockSupressException() => action switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ?
        new ActionBlock<TInput>(input =>
        {
            try
            {
                delegateAction(input);
            }
            catch
            {
                // TODO: trace
            }
        }) :
        new ActionBlock<TInput>(input =>
        {
            try
            {
                delegateAction(input);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions),

        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ?
        new ActionBlock<TInput>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }) :
        new ActionBlock<TInput>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<TInput, TInput> CreatePropagatorBlock() => action switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TInput>(input =>
        {
            delegateAction(input);

            return input;
        }) :
        new TransformBlock<TInput, TInput>(input =>
        {
            delegateAction(input);

            return input;
        }, dataflowBlockOptions),

        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TInput>(async input =>
        {
            await delegateTask(input).ConfigureAwait(false);

            return input;
        }) :
        new TransformBlock<TInput, TInput>(async input =>
        {
            await delegateTask(input).ConfigureAwait(false);

            return input;
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<TInput, TInput> CreatePropagatorBlockSupressException() => action switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TInput>(input =>
        {
            try
            {
                delegateAction(input);
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }) :
        new TransformBlock<TInput, TInput>(input =>
        {
            try
            {
                delegateAction(input);
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }, dataflowBlockOptions),

        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TInput>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }) :
        new TransformBlock<TInput, TInput>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);
            }
            catch
            {
                // TODO: trace
            }

            return input;
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };

    public TransformBlock<TInput, ActionResult<TInput>> CreateResultPropagatorBlock() => action switch
    {
        Action<TInput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                delegateAction(input);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }) :
        new TransformBlock<TInput, ActionResult<TInput>>(input =>
        {
            try
            {
                delegateAction(input);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions),

        Func<TInput, Task> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }) :
        new TransformBlock<TInput, ActionResult<TInput>>(async input =>
        {
            try
            {
                await delegateTask(input).ConfigureAwait(false);

                return new ActionResult<TInput>(input);
            }
            catch (Exception ex)
            {
                return new ActionResult<TInput>(input, ex);
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(action)} is an invalid type."),
    };
}

public sealed class TransformBlockBuilder<TInput, TOutput>
{
    private readonly Delegate transform;
    private readonly ExecutionDataflowBlockOptions? dataflowBlockOptions;

    public TransformBlockBuilder(Func<TInput, TOutput> transform!!)
    {
        this.transform = transform;
    }
    public TransformBlockBuilder(Func<TInput, TOutput> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.transform = transform;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }
    public TransformBlockBuilder(Func<TInput, Task<TOutput>> transform!!)
    {
        this.transform = transform;
    }
    public TransformBlockBuilder(Func<TInput, Task<TOutput>> transform!!, ExecutionDataflowBlockOptions dataflowBlockOptions!!)
    {
        this.transform = transform;
        this.dataflowBlockOptions = dataflowBlockOptions;
    }

    public TransformBlock<TInput, TOutput> CreateTransformBlock() => transform switch
    {
        Func<TInput, TOutput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TOutput>(delegateAction) :
        new TransformBlock<TInput, TOutput>(delegateAction, dataflowBlockOptions),

        Func<TInput, Task<TOutput>> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TOutput>(delegateTask) :
        new TransformBlock<TInput, TOutput>(delegateTask, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(transform)} is an invalid type."),
    };

    public TransformBlock<TInput, TransformResult<TInput, TOutput>> CreateResultPropagatorBlock() => transform switch
    {
        Func<TInput, TOutput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TransformResult<TInput, TOutput>>(item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(delegateAction(item));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }) :
        new TransformBlock<TInput, TransformResult<TInput, TOutput>>(item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(delegateAction(item));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }, dataflowBlockOptions),

        Func<TInput, Task<TOutput>> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TransformResult<TInput, TOutput>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(await delegateTask(item).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }) :
        new TransformBlock<TInput, TransformResult<TInput, TOutput>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, TOutput>(await delegateTask(item).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, TOutput>(item, ex);
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(transform)} is an invalid type."),
    };

    public TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>> CreateItemResultPropagatorBlock() => transform switch
    {
        Func<TInput, TOutput> delegateAction => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, delegateAction(item)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }) :
        new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, delegateAction(item)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }, dataflowBlockOptions),

        Func<TInput, Task<TOutput>> delegateTask => dataflowBlockOptions is null ?
        new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, await delegateTask(item).ConfigureAwait(false)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }) :
        new TransformBlock<TInput, TransformResult<TInput, ItemResult<TInput, TOutput>>>(async item =>
        {
            try
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(new ItemResult<TInput, TOutput>(item, await delegateTask(item).ConfigureAwait(false)));
            }
            catch (Exception ex)
            {
                return new TransformResult<TInput, ItemResult<TInput, TOutput>>(item, ex);
            }
        }, dataflowBlockOptions),

        _ => throw new InvalidOperationException($"{nameof(transform)} is an invalid type."),
    };
}
