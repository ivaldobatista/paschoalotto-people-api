using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paschoalotto.People.Api.Contracts;
using Paschoalotto.People.Api.Contracts.Responses;
using Paschoalotto.People.Application.Abstractions;
using Paschoalotto.People.Application.Abstractions.Repositories;
using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Domain.People.ValueObjects;

namespace Paschoalotto.People.Api.Controllers.V1;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
[Authorize]
public sealed class PeopleController : ControllerBase
{
    private readonly IPersonWriteRepository _write;
    private readonly IPersonReadRepository _read;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<PeopleController> _logger;
    private readonly IFileStorageService _storage;
    private readonly IAuditLogger _audit;

    public PeopleController(
        IPersonWriteRepository write,
        IPersonReadRepository read,
        IUnitOfWork uow,
        IFileStorageService storageService,
        IAuditLogger audit,
        ILogger<PeopleController> logger)
    {
        _write = write;
        _read = read;
        _uow = uow;
        _logger = logger;
        _storage = storageService;
        _audit = audit;
    }

    [HttpPost("individuals")]
    [ProducesResponseType(typeof(IndividualResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var entity = new Individual(
                fullName: req.FullName,
                cpf: new Cpf(req.Cpf),
                birthDate: req.BirthDate,
                gender: req.Gender,
                email: new EmailAddress(req.Email),
                phone: new PhoneNumber(req.Phone),
                address: new Address(
                    req.Address.Street, req.Address.Number, req.Address.Complement,
                    req.Address.District, req.Address.City, req.Address.State,
                    req.Address.ZipCode, req.Address.Country)
            );

            await _write.AddIndividualAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            _audit.Log("Create", "Individual", entity.Id.ToString(), new { entity.FullName });

            var resp = new IndividualResponse
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Cpf = entity.Cpf.Value,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Email = entity.Email.Value,
                Phone = entity.Phone.Value,
                Street = entity.Address.Street,
                Number = entity.Address.Number,
                Complement = entity.Address.Complement,
                District = entity.Address.District,
                City = entity.Address.City,
                State = entity.Address.State,
                ZipCode = entity.Address.ZipCode,
                Country = entity.Address.Country,
                PhotoPath = entity.PhotoPath
            };

            _logger.LogInformation("Pessoa física criada com sucesso. Id={PersonId}", entity.Id);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resp);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validação de domínio falhou ao criar pessoa física.");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa física.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erro interno ao criar pessoa física." });
        }
    }

    [HttpPost("legal-entities")]
    [ProducesResponseType(typeof(LegalEntityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLegalEntity([FromBody] CreateLegalEntityRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var entity = new LegalEntity(
                corporateName: req.CorporateName,
                tradeName: req.TradeName,
                cnpj: new Cnpj(req.Cnpj),
                stateRegistration: req.StateRegistration,
                municipalRegistration: req.MunicipalRegistration,
                legalRepName: req.LegalRepresentativeName,
                legalRepCpf: new Cpf(req.LegalRepresentativeCpf),
                email: new EmailAddress(req.Email),
                phone: new PhoneNumber(req.Phone),
                address: new Address(
                    req.Address.Street, req.Address.Number, req.Address.Complement,
                    req.Address.District, req.Address.City, req.Address.State,
                    req.Address.ZipCode, req.Address.Country)
            );

            await _write.AddLegalEntityAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            _audit.Log("Create", "LegalEntity", entity.Id.ToString(), new { entity.CorporateName });

            var resp = new LegalEntityResponse
            {
                Id = entity.Id,
                CorporateName = entity.CorporateName,
                TradeName = entity.TradeName,
                Cnpj = entity.Cnpj.Value,
                StateRegistration = entity.StateRegistration,
                MunicipalRegistration = entity.MunicipalRegistration,
                LegalRepresentativeName = entity.LegalRepresentativeName,
                LegalRepresentativeCpf = entity.LegalRepresentativeCpf.Value,
                Email = entity.Email.Value,
                Phone = entity.Phone.Value,
                Street = entity.Address.Street,
                Number = entity.Address.Number,
                Complement = entity.Address.Complement,
                District = entity.Address.District,
                City = entity.Address.City,
                State = entity.Address.State,
                ZipCode = entity.Address.ZipCode,
                Country = entity.Address.Country,
                LogoPath = entity.LogoPath
            };

            _logger.LogInformation("Pessoa jurídica criada com sucesso. Id={PersonId}", entity.Id);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resp);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validação de domínio falhou ao criar pessoa jurídica.");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa jurídica.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erro interno ao criar pessoa jurídica." });
        }
    }

    [HttpGet("people/{id:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var person = await _read.GetByIdAsync(id, ct);
        if (person is null) return NotFound();

        if (person is Individual i)
        {
            var resp = new IndividualResponse
            {
                Id = i.Id,
                FullName = i.FullName,
                Cpf = i.Cpf.Value,
                BirthDate = i.BirthDate,
                Gender = i.Gender,
                Email = i.Email.Value,
                Phone = i.Phone.Value,
                Street = i.Address.Street,
                Number = i.Address.Number,
                Complement = i.Address.Complement,
                District = i.Address.District,
                City = i.Address.City,
                State = i.Address.State,
                ZipCode = i.Address.ZipCode,
                Country = i.Address.Country,
                PhotoPath = i.PhotoPath,
                PhotoUrl = string.IsNullOrEmpty(i.PhotoPath) ? null : $"{baseUrl}/files/{i.PhotoPath}"
            };
            return Ok(resp);
        }

        if (person is LegalEntity j)
        {
            var resp = new LegalEntityResponse
            {
                Id = j.Id,
                CorporateName = j.CorporateName,
                TradeName = j.TradeName,
                Cnpj = j.Cnpj.Value,
                StateRegistration = j.StateRegistration,
                MunicipalRegistration = j.MunicipalRegistration,
                LegalRepresentativeName = j.LegalRepresentativeName,
                LegalRepresentativeCpf = j.LegalRepresentativeCpf.Value,
                Email = j.Email.Value,
                Phone = j.Phone.Value,
                Street = j.Address.Street,
                Number = j.Address.Number,
                Complement = j.Address.Complement,
                District = j.Address.District,
                City = j.Address.City,
                State = j.Address.State,
                ZipCode = j.Address.ZipCode,
                Country = j.Address.Country,
                LogoPath = j.LogoPath,
                LogoUrl = string.IsNullOrEmpty(j.LogoPath) ? null : $"{baseUrl}/files/{j.LogoPath}"
            };
            return Ok(resp);
        }

        return NotFound();
    }

    [HttpGet("people/search")]
    [ProducesResponseType(typeof(IEnumerable<PersonSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string name, CancellationToken ct)
    {
        name = (name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            return Ok(Array.Empty<PersonSummaryResponse>());

        var people = await _read.SearchByNameAsync(name, 50, ct);

        var list = new List<PersonSummaryResponse>(people.Count);
        foreach (var p in people)
        {
            if (p is Individual i)
            {
                list.Add(new PersonSummaryResponse
                {
                    Id = i.Id,
                    Kind = "individual",
                    DisplayName = i.FullName
                });
            }
            else if (p is LegalEntity j)
            {
                list.Add(new PersonSummaryResponse
                {
                    Id = j.Id,
                    Kind = "legalEntity",
                    DisplayName = string.IsNullOrWhiteSpace(j.TradeName) ? j.CorporateName : j.TradeName
                });
            }
        }

        return Ok(list);
    }

    [HttpPost("individuals/{id:guid}/photo")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadIndividualPhoto([FromRoute] Guid id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "Arquivo de foto é obrigatório." });

        var ext = Path.GetExtension(file.FileName);
        await using var stream = file.OpenReadStream();
        var relPath = await _storage.SaveAsync(stream, ext, "photos", $"individual-{id}", ct);

        var updated = await _write.UpdateIndividualPhotoAsync(id, relPath, ct);
        if (!updated) return NotFound(new { error = "Pessoa física não encontrada." });

        await _uow.SaveChangesAsync(ct);
        _audit.Log("UploadPhoto", "Individual", id.ToString(), new { path = relPath });

        var photoUrl = $"{Request.Scheme}://{Request.Host}/files/{relPath}";
        Response.Headers.Location = photoUrl;
        return NoContent();
    }

    [HttpPost("legal-entities/{id:guid}/logo")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadLegalEntityLogo([FromRoute] Guid id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "Arquivo de logotipo é obrigatório." });

        var ext = Path.GetExtension(file.FileName);
        await using var stream = file.OpenReadStream();
        var relPath = await _storage.SaveAsync(stream, ext, "logos", $"legal-{id}", ct);

        var updated = await _write.UpdateLegalEntityLogoAsync(id, relPath, ct);
        if (!updated) return NotFound(new { error = "Pessoa jurídica não encontrada." });

        await _uow.SaveChangesAsync(ct);
        _audit.Log("UploadLogo", "LegalEntity", id.ToString(), new { path = relPath });

        var logoUrl = $"{Request.Scheme}://{Request.Host}/files/{relPath}";
        Response.Headers.Location = logoUrl;
        return NoContent();
    }

}
