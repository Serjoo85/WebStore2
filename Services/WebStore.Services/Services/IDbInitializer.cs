namespace WebStore.Services.Services;

public interface IDbInitializer
{
    Task<bool> RemoveAsync(CancellationToken cancel = default);
    Task InitializeAsync(bool removeBefore = false, CancellationToken cancel = default);
}