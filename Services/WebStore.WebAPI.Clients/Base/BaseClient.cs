using System.Net;
using System.Net.Http.Json;

namespace WebStore.WebAPI.Clients.Base;

public abstract class BaseClient : IDisposable
{
    protected HttpClient Http { get; }
    protected string Address { get; }

    protected BaseClient(HttpClient client, string address)
    {
        Http = client;
        Address = address;
    }

    protected T? Get<T>(string url) => GetAsync<T>(url).Result;
    protected async Task<T?> GetAsync<T>(string url, CancellationToken cancel = default)
    {
        var response = await Http.GetAsync(url, cancel).ConfigureAwait(false);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.NotFound:
                return default;
            default:
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<T>(cancellationToken:cancel)
                    .ConfigureAwait(false);
                return result;
        }
    }

    protected HttpResponseMessage Post<T>(string url, T value, CancellationToken cancel = default) => PostAsync(url, value, cancel).Result;

    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T value, CancellationToken cancel = default)
    {
        var response = await Http.PostAsJsonAsync(url, value, cancel).ConfigureAwait(false);
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Put<T>(string url, T value) => PutAsync(url, value).Result;

    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value, CancellationToken cancel = default)
    {
        var response = await Http.PutAsJsonAsync(url, value, cancel).ConfigureAwait(false);
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

    protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
    {
        var response = await Http.DeleteAsync(url, cancel).ConfigureAwait(false);
        return response;
    }

    //~BaseClient() => Dispose(false);

    public void Dispose()
    {
        if(_disposed) return;
        Dispose(true);
        _disposed = true;
        // При наличии destructor чуть-чуть ускоряет процесс исключая из очереди на finalization.
        //GC.SuppressFinalize(this);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if(_disposed) return;
        if (disposing)
        {
            // Необходимо освободить все управляемые ресурсы -> вызывать Dispose() везде где нужно.
            // Http.Dispose(); -> Уничтожать нельзя, т.к. не мы его создали!!!
        }

        // освобождение неуправляемых ресурсов.
    }
}