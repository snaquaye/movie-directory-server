namespace MovieDirectory.Endpoints
{
    public class IndexResponse
    {
        public string Message { get; set; } = default!;
    }

    public class IndexEndpoint : EndpointWithoutRequest<IndexResponse>
    {
        public override void Configure()
        {
            Get("/");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendOkAsync(new IndexResponse
            {
                Message = "Hello Fast Endpoints"
            }, ct);
        }
    }
}