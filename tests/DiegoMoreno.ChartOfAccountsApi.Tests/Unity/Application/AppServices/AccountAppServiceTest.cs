using AutoMapper;
using Bogus;
using DiegoMoreno.ChartOfAccountsApi.Application.AppServices;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Net;

namespace DiegoMoreno.ChartOfAccountsApi.Tests.Unity.Application.AppServices;
public class AccountAppServiceTest
{
    private readonly Faker _faker;
    private readonly Mock<IAccountService> _accountService;
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<IAccountTypeRepository> _accountTypeRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly AccountAppService _accountAppService;

    public AccountAppServiceTest()
    {
        _faker = new Faker();
        var mocker = new AutoMocker();

        _accountService = mocker.GetMock<IAccountService>();
        _accountRepository = mocker.GetMock<IAccountRepository>();
        _accountTypeRepository = mocker.GetMock<IAccountTypeRepository>();
        _mapper = mocker.GetMock<IMapper>();

        _accountAppService = mocker.CreateInstance<AccountAppService>();
    }

    [Fact]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenCodeGroupIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            _faker.Random.AlphaNumeric(5), _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }
}
