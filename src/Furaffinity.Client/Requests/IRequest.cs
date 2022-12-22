namespace Furaffinity.Client.Requests;

internal interface IRequest : IDisposable
{
    HttpRequestMessage RequestMessage { get; }
}