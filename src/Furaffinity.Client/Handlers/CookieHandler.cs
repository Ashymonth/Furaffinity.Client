﻿using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers.Errors;

namespace Furaffinity.Client.Handlers;

internal class CookieHandler : DelegatingHandler
{
    private readonly UnauthorizedPageParser _pageParser;
    
    public CookieHandler(UnauthorizedPageParser pageParser)
    {
        _pageParser = pageParser;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Options.TryGetValue(new HttpRequestOptionsKey<string>("Cookie"), out string? cookies))
        {
            throw new AuthorizationCookieException("Cookies are required for requests");
        }

        if (string.IsNullOrWhiteSpace(cookies))
        {
            throw new AuthorizationCookieException("Cookies can't be empty");
        }

        request.Headers.TryAddWithoutValidation("Cookie", cookies);

        var response = await base.SendAsync(request, cancellationToken);

        await response.Content.LoadIntoBufferAsync();

        var page = await response.Content.ReadAsStringAsync(cancellationToken);

        //check, that provided cookie are valid and we pass auth.
        _pageParser.ValidatePage(page);

        return response;
    }
}