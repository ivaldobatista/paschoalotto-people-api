using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
public sealed class PeopleController : ControllerBase
{
    private readonly IPersonWriteRepository _write;
    private readonly IPersonReadRepository _read;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(
        IPersonWriteRepository write,
        IPersonReadRepository read,
        IUnitOfWork uow,
        ILogger<PeopleController> logger)
    {
        _write = write;
        _read = read;
        _uow = uow;
        _logger = logger;
    }

    // === POST /api/v1/individuals
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
                    req.Address.ZipCode, req.Address.Country),
                photoPath: req.PhotoPath
            );

            await _write.AddIndividualAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

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

    // === POST /api/v1/legal-entities
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
                    req.Address.ZipCode, req.Address.Country),
                logoPath: req.LogoPath
            );

            await _write.AddLegalEntityAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

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

    // === GET /api/v1/people/{id}
    [HttpGet("people/{id:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
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
                PhotoPath = i.PhotoPath
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
                LogoPath = j.LogoPath
            };
            return Ok(resp);
        }

        return NotFound();
    }

    // === GET /api/v1/people/search?name=...
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
}
