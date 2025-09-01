namespace Paschoalotto.People.Api.Contracts.Responses;

public sealed class PersonSummaryResponse
{
    public Guid Id { get; set; }
    public string Kind { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
}
