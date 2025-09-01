using FluentAssertions;
using Paschoalotto.People.Api.Contracts;
using Paschoalotto.People.Api.Contracts.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class LegalEntitiesFlowTests : IClassFixture<PeopleApiFactory>
{
    private readonly HttpClient _client;
    public LegalEntitiesFlowTests(PeopleApiFactory factory) => _client = factory.CreateClient(new() { AllowAutoRedirect = false });

    [Fact]
    public async Task Create_UploadLogo_GetById_Should_Work()
    {
        var token = await AuthTestHelper.LoginAndGetTokenAsync(_client, "admin", "123456");
        _client.UseBearer(token);

        var body = new CreateLegalEntityRequest
        {
            CorporateName = "Acme LTDA",
            TradeName = "Acme",
            Cnpj = "00504288000131",
            StateRegistration = "ISENTO",
            MunicipalRegistration = "123",
            LegalRepresentativeName = "João Souza",
            LegalRepresentativeCpf = "11144477735",
            Email = "contato@acme.com",
            Phone = "+556199999",
            Address = new AddressDto { Street = "Av. B", Number = "100", Complement = "Sala 3", District = "Centro", City = "Brasília", State = "DF", ZipCode = "70000-000", Country = "Brasil" }
        };

        var createResp = await _client.PostAsJsonAsync("/api/v1/legal-entities", body);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResp.Content.ReadFromJsonAsync<LegalEntityResponse>();
        var id = created!.Id;

        using var content = new MultipartFormDataContent();
        var fileBytes = new byte[] { 0xFF, 0xD8, 0xFF }; 
        content.Add(new ByteArrayContent(fileBytes) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "file", "logo.jpg");

        var upload = await _client.PostAsync($"/api/v1/legal-entities/{id}/logo", content);
        upload.StatusCode.Should().Be(HttpStatusCode.NoContent);
        upload.Headers.Location.Should().NotBeNull();

        var get = await _client.GetAsync($"/api/v1/people/{id}");
        var person = await get.Content.ReadFromJsonAsync<LegalEntityResponse>();
        person!.LogoPath.Should().NotBeNullOrEmpty();
        person.LogoUrl.Should().Contain("/files/");
    }
}
