using DataAccess.Results;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DataAccess.Abstractions
{
    /// <summary>
    /// Base repository for fast implementation of CRUD operations.
    /// Requires clarification for complex entities 
    /// (in such cases, it is better to inherit from IRepository directly)
    /// </summary>
    /// <typeparam name="T">The main entity that the repository works with.</typeparam>
    public abstract class BaseRepository<T> where T : class, IEntity
    {
        private const string EXCEPTION_FORMAT = "{RepositoryName} unexpected error in {OperationName}: {Message}";
        private const string EXCEPTION_MESSAGE = "Unexpected error occured";
        private const string ARGUMENT_EX_FORMAT = "{RepositoryName} argument error in {OperationName}: {Message}";
        private const string DB_UPDATE_EX_FORMAT = "{RepositoryName} database error in {OperationName}: {Message}";
        private const string DB_UPDATE_EX_MESSAGE = "Database error occured";
        private const string DEF_INFO_FORMAT = "{RepositoryName} {OperationName} completed in {ElapsedMs}ms";
        private const string VAL_FAIL_FORMAT = "{RepositoryName} {OperationName} failed validation: {Message}";
        private const string RES_NULL_FORMAT = "{RepositoryName} {OperationName} result was null unexpectedly";
        private const string RES_MESSAGE = "Operation result was invalid. Try again later";
        public IContextManager ContextManager { get; }
        protected ILogger _logger;
        public BaseRepository(IContextManager contextManager, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(contextManager, nameof(contextManager));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ContextManager = contextManager;
            _logger = logger;
        }
        private IResult<TResult> HandleException<TResult>(Stopwatch sw, string operationName, string ex)
        {
            sw.Stop();
            _logger.LogError(EXCEPTION_FORMAT, GetType().Name, operationName, ex);
            return CreateErrorResult<TResult>(EXCEPTION_MESSAGE, sw.ElapsedMilliseconds);
        }
        private IResult<TResult> HandleArgumentException<TResult>(Stopwatch sw, string operationName, string ex)
        {
            sw.Stop();
            _logger.LogWarning(ARGUMENT_EX_FORMAT, GetType().Name, operationName, ex);
            return CreateErrorResult<TResult>(ex, sw.ElapsedMilliseconds);
        }
        private IResult<TResult> HandleDbUpdateException<TResult>(Stopwatch sw, string operationName, string ex)
        {
            sw.Stop();
            _logger.LogWarning(DB_UPDATE_EX_FORMAT, GetType().Name, operationName, ex);
            return CreateErrorResult<TResult>(DB_UPDATE_EX_MESSAGE, sw.ElapsedMilliseconds);
        }
        protected async Task<IResult<TResult>> ExecuteOperation<TResult>(
            Func<ApplicationDbContext, Task<TResult>> operation,
            string operationName)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    var result = await operation(context);
                    sw.Stop();

                    _logger.LogInformation(DEF_INFO_FORMAT, GetType().Name, operationName, sw.ElapsedMilliseconds);

                    return CreateSuccessResult(result, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                return HandleDbUpdateException<TResult>(sw, operationName, dbUpdateException.Message);
            }
            catch (ArgumentException argumentException)
            {
                return HandleArgumentException<TResult>(sw, operationName, argumentException.Message);
            }
            catch (Exception ex)
            {
                return HandleException<TResult>(sw, operationName, ex.Message);
            }
        }
        protected async Task<IResult<TResult>> ExecuteOperation<TResult>(
            Func<ApplicationDbContext, Task<(TResult Result, ValidationResult Validation)>> operation,
            string operationName)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    var (result, validation) = await operation(context);

                    if (!validation.IsValid)
                    {
                        sw.Stop();
                        _logger.LogInformation(VAL_FAIL_FORMAT, GetType().Name, operationName, validation.ErrorMessage);
                        return CreateErrorResult<TResult>(validation.ErrorMessage, sw.ElapsedMilliseconds);
                    }
                    if (result == null)
                    {
                        sw.Stop();
                        _logger.LogWarning(RES_NULL_FORMAT, GetType().Name, operationName);
                        return CreateErrorResult<TResult>(RES_MESSAGE, sw.ElapsedMilliseconds);
                    }
                    sw.Stop();
                    _logger.LogInformation(DEF_INFO_FORMAT, GetType().Name, operationName, sw.ElapsedMilliseconds);

                    return CreateSuccessResult(result, sw.ElapsedMilliseconds);

                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                return HandleDbUpdateException<TResult>(sw, operationName, dbUpdateException.Message);
            }
            catch (ArgumentException argumentException)
            {
                return HandleArgumentException<TResult>(sw, operationName, argumentException.Message);
            }
            catch (Exception ex)
            {
                return HandleException<TResult>(sw, operationName, ex.Message);
            }
        }
        protected virtual IResult<TResult> CreateSuccessResult<TResult>(TResult result, long duration)
        {
            if (result is bool boolResult)
                return (IResult<TResult>)new BoolResult(boolResult, duration);

            if (result is Guid guidResult)
                return (IResult<TResult>)new GuidResult(guidResult, duration);

            if (result is IEntity entityResult)
                return new EntityResult<TResult>((TResult)(object)entityResult, duration);

            return new EntityResult<TResult>(result, duration);
        }
        protected virtual IResult<TResult> CreateErrorResult<TResult>(Exception ex, long duration)
        {
            if (typeof(TResult) == typeof(bool))
                return (IResult<TResult>)new BoolResult(ex, duration);

            if (typeof(TResult) == typeof(Guid))
                return (IResult<TResult>)new GuidResult(ex, duration);

            return new EntityResult<TResult>(ex, duration);
        }
        protected virtual IResult<TResult> CreateErrorResult<TResult>(string exMessage, long duration)
        {
            if (typeof(TResult) == typeof(bool))
                return (IResult<TResult>)new BoolResult(exMessage, duration);

            if (typeof(TResult) == typeof(Guid))
                return (IResult<TResult>)new GuidResult(exMessage, duration);

            return new EntityResult<TResult>(exMessage, duration);
        }
    }
}
