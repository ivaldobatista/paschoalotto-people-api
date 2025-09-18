using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Paschoalotto.People.Api.Contracts;
using Paschoalotto.People.Api.Contracts.Responses;
using Paschoalotto.People.Domain.People.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Paschoalotto.People.UnitTests.Integration;

public class IndividualsFlowTests : IClassFixture<PeopleApiFactory>
{
    private readonly HttpClient _client;

    public IndividualsFlowTests(PeopleApiFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task Create_UploadPhoto_GetById_Should_Work()
    {
        var token = await AuthTestHelper.LoginAndGetTokenAsync(_client, "admin", "123456");
        _client.UseBearer(token);

        var body = new CreateIndividualRequest
        {
            FullName = "Maria Silva",
            Cpf = "11144477735",
            BirthDate = new DateTime(1990, 5, 12),
            Gender = GenderType.Female,
            Email = "maria@example.com",
            Phone = "+556199999",
            Address = new AddressDto { Street = "Rua A", Number = "10", District = "Centro", City = "Brasília", State = "DF", ZipCode = "70000-000", Country = "Brasil" }
        };

        var createResp = await _client.PostAsJsonAsync("/api/v1/individuals", body);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResp.Content.ReadFromJsonAsync<IndividualResponse>();
        created.Should().NotBeNull();
        var id = created!.Id;

        using var content = new MultipartFormDataContent();
        var fileBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // mock PNG header
        content.Add(new ByteArrayContent(fileBytes) { Headers = { ContentType = new MediaTypeHeaderValue("image/png") } }, "file", "photo.png");

        var upload = await _client.PostAsync($"/api/v1/individuals/{id}/photo", content);
        upload.StatusCode.Should().Be(HttpStatusCode.NoContent);
        upload.Headers.Location.Should().NotBeNull();

        var get = await _client.GetAsync($"/api/v1/people/{id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
        var person = await get.Content.ReadFromJsonAsync<IndividualResponse>();
        person!.PhotoPath.Should().NotBeNullOrEmpty();
        person.PhotoUrl.Should().Contain("/files/");
    }
}
