using AutoMapper;
using Bogus;
using DiegoMoreno.ChartOfAccountsApi.Application.AppServices;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
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

    [Fact(DisplayName = "AddAsync should create an account when all information is valid")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenAllRight_ShouldCreateAccount()
    {
        // Arrange
        var firstCode = _faker.Random.Int(1, 999);
        var secondCode = _faker.Random.Int(1, 999);

        var request = new AddAccountRequestDto(null,
            $"{firstCode}", _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = secondCode,
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        var accountResponse =
            new AddAccountResponseDto(account.Id, account.IdParentAccount, account.Code,
            account.Name, account.IdAccountType, account.AcceptEntries);

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _mapper
            .Setup(m => m.Map<AddAccountResponseDto>(It.IsAny<Account>()))
            .Returns(accountResponse);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());
        _accountRepository.Setup(a => a.AddAsync(It.IsAny<Account>())).ReturnsAsync(account);

        _accountRepository.Setup(a => a.CommitAsync()).ReturnsAsync(1);


        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.Created);
        response.Data.Should().BeEquivalentTo(accountResponse);
        _accountRepository
            .Verify(v => v.AddAsync(It.Is<Account>(a => a.Code == account.Code &&
                                                        a.AcceptEntries == account.AcceptEntries &&
                                                        a.IdParentAccount == account.IdParentAccount &&
                                                        a.IdAccountType == account.IdAccountType &&
                                                        a.Name == account.Name))
            , Times.Once);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when the code group is invalid")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenCodeGroupIsInvalid_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            _faker.Random.AlphaNumeric(5), _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when the account type does not exist")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenThereIsntTheAccountType_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            _faker.Random.Int(1, 999).ToString(), _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when the code group already exists")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenTheCodeGroupAlreadyExists_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            _faker.Random.Int(1, 999).ToString(), _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = Convert.ToInt32(request.CodeGroup),
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        _accountRepository
            .Setup(a => a.GetAccountByParentAndCodeAsync(account.IdParentAccount, account.Code))
            .ReturnsAsync(new Account());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when the parent account does not exist")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenTheParentDoesnotExist_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            _faker.Random.Int(1, 999).ToString(), _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = Convert.ToInt32(request.CodeGroup),
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when the parent account accepts entries")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenTheParentAcceptEntries_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var firstCode = _faker.Random.Int(1, 999);
        var secondCode = _faker.Random.Int(1, 999);
        var request = new AddAccountRequestDto(Guid.NewGuid(),
            $"{firstCode}.{secondCode}", _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var accountParent = new Account
        {
            Id = request.IdParentAccount!.Value,
            Code = firstCode,
            AcceptEntries = true
        };

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = secondCode,
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        _accountRepository
            .Setup(a => a.GetByIdAsync(request.IdParentAccount!.Value))
            .ReturnsAsync(accountParent);

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return BadRequest and not persist when it has a parent and an invalid code group")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenItHasParentAndInvalidCodeGroup_ShouldReturnBadRequestAndNotPersist()
    {
        // Arrange
        var firstCode = _faker.Random.Int(1, 999);
        var secondCode = _faker.Random.Int(1, 999);
        var otherCode = _faker.Random.Int(1, 999);

        var request = new AddAccountRequestDto(Guid.NewGuid(),
            $"{firstCode}.{secondCode}.{otherCode}", _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var accountParent = new Account
        {
            Id = request.IdParentAccount!.Value,
            Code = firstCode,
            AcceptEntries = false
        };

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = secondCode,
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        _accountRepository
            .Setup(a => a.GetByIdAsync(request.IdParentAccount!.Value))
            .ReturnsAsync(accountParent);

        _accountService
            .Setup(a => a.GetCodeGroupAsync(It.IsAny<Account>()))
            .ReturnsAsync($"{firstCode}.{secondCode}");

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
        _accountRepository.Verify(a => a.AddAsync(It.IsAny<Account>()), Times.Never);
        _accountRepository.Verify(a => a.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AddAsync should return InternalServerError when an exception occurs")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenHaveAnException_ShouldReturnInternalServerError()
    {
        // Arrange
        var firstCode = _faker.Random.Int(1, 999);
        var secondCode = _faker.Random.Int(1, 999);

        var request = new AddAccountRequestDto(Guid.NewGuid(),
            $"{firstCode}.{secondCode}", _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var accountParent = new Account
        {
            Id = request.IdParentAccount!.Value,
            Code = firstCode,
            AcceptEntries = false
        };

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = secondCode,
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        _accountRepository
            .Setup(a => a.GetByIdAsync(request.IdParentAccount!.Value))
            .ReturnsAsync(accountParent);

        _accountService
            .Setup(a => a.GetCodeGroupAsync(It.IsAny<Account>()))
            .ReturnsAsync($"{firstCode}.{secondCode}");

        _accountRepository.Setup(a => a.AddAsync(It.IsAny<Account>())).ThrowsAsync(new Exception());

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "AddAsync should return InternalServerError when CommitAsync returns less than one")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task Add_WhenCommitReturnLessThanOne_ShouldReturnInternalServerError()
    {
        // Arrange
        var firstCode = _faker.Random.Int(1, 999);

        var request = new AddAccountRequestDto(null,
            $"{firstCode}", _faker.Lorem.Word(),
            Guid.NewGuid(), _faker.Random.Bool());

        var account = new Account
        {
            AcceptEntries = request.AcceptEntries,
            Code = firstCode,
            Id = request.Id,
            IdAccountType = request.IdAccountType,
            IdParentAccount = request.IdParentAccount,
            Name = request.Name
        };

        _mapper
            .Setup(m => m.Map<Account>(It.IsAny<AddAccountRequestDto>()))
            .Returns(account);

        _accountTypeRepository
            .Setup(a => a.GetByIdAsync(request.IdAccountType))
            .ReturnsAsync(new AccountType());

        _accountRepository.Setup(a => a.AddAsync(It.IsAny<Account>())).ReturnsAsync(account);

        _accountRepository.Setup(a => a.CommitAsync()).ReturnsAsync(0);

        // Act
        var response = await _accountAppService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }




    [Fact(DisplayName = "DeleteAsync should delete the account when all conditions are met")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task Delete_WhenAllRight_ShouldDeleteAccount()
    {
        // Arrange
        var request = new DeleteAccountRequestDto(Guid.NewGuid());

        var account = new Account { Id = request.Id };

        _accountRepository.Setup(a => a.GetByIdAsync(request.Id)).ReturnsAsync(account);
        _accountRepository.Setup(a => a.CommitAsync()).ReturnsAsync(1);
        _accountRepository.Setup(a => a.GetAccountChildrenAsync(account.Id)).ReturnsAsync([]);

        // Act
        var response = await _accountAppService.DeleteAsync(request);

        // Assert
        response.Data.Should().BeTrue();
        _accountRepository.Verify(a => a.DeleteAsync(account.Id), Times.Once);
    }

    [Fact(DisplayName = "DeleteAsync should return false when the account does not exist")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task Delete_WhenAccountNotExist_ShouldReturnFalse()
    {
        // Arrange
        var request = new DeleteAccountRequestDto(Guid.NewGuid());

        // Act
        var response = await _accountAppService.DeleteAsync(request);

        // Assert
        response.Data.Should().BeFalse();
    }

    [Fact(DisplayName = "DeleteAsync should return false when the account has children")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task Delete_WhenAccountThereAreChildren_ShouldReturnFalse()
    {
        // Arrange
        var request = new DeleteAccountRequestDto(Guid.NewGuid());

        var account = new Account { Id = request.Id };

        _accountRepository.Setup(a => a.GetByIdAsync(request.Id)).ReturnsAsync(account);
        _accountRepository.Setup(a => a.GetAccountChildrenAsync(account.Id)).ReturnsAsync([new()]);

        // Act
        var response = await _accountAppService.DeleteAsync(request);

        // Assert
        response.Data.Should().BeFalse();
    }

    [Fact(DisplayName = "DeleteAsync should return false when an exception occurs")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task Delete_WhenHaveAnException_ShouldReturnFalse()
    {
        // Arrange
        var request = new DeleteAccountRequestDto(Guid.NewGuid());

        var account = new Account { Id = request.Id };

        _accountRepository.Setup(a => a.GetByIdAsync(request.Id)).ReturnsAsync(account);
        _accountRepository.Setup(a => a.GetAccountChildrenAsync(account.Id)).ReturnsAsync([]);
        _accountRepository.Setup(a => a.DeleteAsync(account.Id)).ThrowsAsync(new Exception());

        // Act
        var response = await _accountAppService.DeleteAsync(request);

        // Assert
        response.Data.Should().BeFalse();
    }

    [Fact(DisplayName = "DeleteAsync should return false when CommitAsync returns less than one")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task Delete_WhenCommitReturnLessThanOne_ShouldReturnFalse()
    {
        // Arrange
        var request = new DeleteAccountRequestDto(Guid.NewGuid());

        var account = new Account { Id = request.Id };

        _accountRepository.Setup(a => a.GetByIdAsync(request.Id)).ReturnsAsync(account);
        _accountRepository.Setup(a => a.GetAccountChildrenAsync(account.Id)).ReturnsAsync([]);
        _accountRepository.Setup(a => a.CommitAsync()).ReturnsAsync(0);

        // Act
        var response = await _accountAppService.DeleteAsync(request);

        // Assert
        response.Data.Should().BeFalse();
    }



    [Fact(DisplayName = "GetSuggestedCode should return the suggested code when the next code is generated successfully")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetSuggestedCodeAsync))]
    public async Task GetSuggestedCode_WhenNextCodeWasGenerated_ShouldReturnSuggestedCode()
    {
        // Arrange
        var request = new NextAccountCodeRequestDto(Guid.NewGuid());
        var suggestedCodeExpected = $"{_faker.Random.Int(1, 999)}.{_faker.Random.Int(1, 999)}.{_faker.Random.Int(1, 999)}";

        _accountService
            .Setup(a => a.CalculateNextCodeAsync(request.IdParentAccount))
            .ReturnsAsync(suggestedCodeExpected);

        // Act
        var suggestedCode = await _accountAppService.GetSuggestedCodeAsync(request);

        // Assert
        suggestedCode.Data!.SuggestedCode.Should().Be(suggestedCodeExpected);
        suggestedCode.Data!.NewParent.Should().BeNull();
    }

    [Fact(DisplayName = "GetSuggestedCode should return NoContent when the new parent code is whitespace")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetSuggestedCodeAsync))]
    public async Task GetSuggestedCode_WhenNewParentCodeIsWhiteSpace_ShouldReturnNoContent()
    {
        // Arrange
        var request = new NextAccountCodeRequestDto(Guid.NewGuid());

        // Act
        var suggestedCode = await _accountAppService.GetSuggestedCodeAsync(request);

        // Assert
        suggestedCode.Code.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "GetSuggestedCode should return the new parent information when a new parent code is generated")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetSuggestedCodeAsync))]
    public async Task GetSuggestedCode_WhenNewParentCodeGenerated_ShouldReturnNewParent()
    {
        // Arrange
        var request = new NextAccountCodeRequestDto(Guid.NewGuid());
        var newSuggestedCodeExpected = $"{_faker.Random.Int(1, 999)}.{_faker.Random.Int(1, 999)}";
        var idAccountExpected = Guid.NewGuid();

        _accountService
            .Setup(a => a.CalculateNextNewParentCodeAsync(request.IdParentAccount!.Value))
            .ReturnsAsync((newSuggestedCodeExpected, idAccountExpected));

        // Act
        var suggestedCode = await _accountAppService.GetSuggestedCodeAsync(request);

        // Assert
        suggestedCode.Data!.SuggestedCode.Should().Be(newSuggestedCodeExpected);
        suggestedCode.Data!.NewParent.Should().Be(idAccountExpected);
    }
}