using Bogus;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Domain.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;

namespace DiegoMoreno.ChartOfAccountsApi.Tests.Unity.Domain.Services;
public class AccountServiceTest
{
    private readonly Faker _faker;
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly AccountService _accountService;

    public AccountServiceTest()
    {
        _faker = new();
        var mocker = new AutoMocker();

        _accountRepository = mocker.GetMock<IAccountRepository>();
        _accountService = mocker.CreateInstance<AccountService>();
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenParentAccountIdIsNull_ShouldReturn1()
    {
        // Assert
        var expectedCodeGroup = "1";

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(null);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task AccountService_WhenThereIsAccount_ShouldReturn1()
    {
        // Assert
        var expectedCodeGroup = "1";

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(Guid.NewGuid());

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenThereArentChild_ShouldReturnCodeGroupCorrectly()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();
        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        var expectedCodeGroup = $"{parentAccount.Code}.1";

        _accountRepository.Setup(a => a.GetByIdAsync(parentAccountId)).ReturnsAsync(parentAccount);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenThereAreOneChild_ShouldReturnCodeGroupCorrectly()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();
        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        var childAccount = new Account
        {
            Code = _faker.Random.Short(1, 998),
            Id = Guid.NewGuid(),
            IdAccountType = Guid.NewGuid(),
            IdParentAccount = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        var expectedCodeGroup = $"{parentAccount.Code}.{childAccount.Code + 1}";

        _accountRepository
            .Setup(a => a.GetByIdAsync(parentAccountId))
            .ReturnsAsync(parentAccount);
        _accountRepository
            .Setup(a => a.GetAccountChildrenAsync(parentAccount.Id))
            .ReturnsAsync([childAccount]);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenThereAreManyChildren_ShouldReturnCodeGroupCorrectly()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();
        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        List<Account> childrenAccounts = [];
        var countChildren = _faker.Random.Int(2, 10);
        var startCode = _faker.Random.Short(1, 988);

        for (int i = 0; i < countChildren; i++)
        {
            childrenAccounts.Add(new()
            {
                Code = startCode++,
                Id = Guid.NewGuid(),
                IdAccountType = Guid.NewGuid(),
                IdParentAccount = parentAccount.Id,
                Name = _faker.Lorem.Word()
            });
        }

        var childAccount = childrenAccounts.OrderBy(c => c.Code).LastOrDefault();

        var expectedCodeGroup = $"{parentAccount.Code}.{childAccount.Code + 1}";

        _accountRepository
            .Setup(a => a.GetByIdAsync(parentAccountId))
            .ReturnsAsync(parentAccount);
        _accountRepository
            .Setup(a => a.GetAccountChildrenAsync(parentAccount.Id))
            .ReturnsAsync(childrenAccounts);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenThereIsHigherLevel_ShouldReturnCodeGroupCorrectly()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();

        var grandParentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = Guid.NewGuid(),
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word(),
            IdParentAccount = grandParentAccount.Id
        };

        var childAccount = new Account
        {
            Code = _faker.Random.Short(1, 998),
            Id = Guid.NewGuid(),
            IdAccountType = Guid.NewGuid(),
            IdParentAccount = parentAccount.Id,
            Name = _faker.Lorem.Word()
        };

        var expectedCodeGroup = $"{grandParentAccount.Code}.{parentAccount.Code}.{childAccount.Code + 1}";

        _accountRepository
            .Setup(a => a.GetByIdAsync(parentAccountId))
            .ReturnsAsync(parentAccount);
        _accountRepository
            .Setup(a => a.GetAccountChildrenAsync(parentAccount.Id))
            .ReturnsAsync([childAccount]);
        _accountRepository
            .Setup(a => a.GetByIdAsync(parentAccount.IdParentAccount.Value))
            .ReturnsAsync(grandParentAccount);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenThereAreHigherLevels_ShouldReturnCodeGroupCorrectly()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();

        var countOfAncestralRelatives = _faker.Random.Int(2, 10);
        var expectedCodeGroup = "";

        Guid? grandParentAccountId = null;

        for (int i = 0; i < countOfAncestralRelatives; i++)
        {
            var grandParentAccount = new Account
            {
                Code = _faker.Random.Short(1, 999),
                Id = Guid.NewGuid(),
                IdAccountType = Guid.NewGuid(),
                Name = _faker.Lorem.Word(),
                IdParentAccount = grandParentAccountId
            };

            grandParentAccountId = grandParentAccount.Id;

            _accountRepository
                .Setup(a => a.GetByIdAsync(grandParentAccount.Id))
                .ReturnsAsync(grandParentAccount);
            expectedCodeGroup += $"{grandParentAccount.Code}.";
        }

        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word(),
            IdParentAccount = grandParentAccountId
        };

        var childAccount = new Account
        {
            Code = _faker.Random.Short(1, 998),
            Id = Guid.NewGuid(),
            IdAccountType = Guid.NewGuid(),
            IdParentAccount = parentAccount.Id,
            Name = _faker.Lorem.Word()
        };

        expectedCodeGroup += $"{parentAccount.Code}.{childAccount.Code + 1}";

        _accountRepository
            .Setup(a => a.GetByIdAsync(parentAccountId))
            .ReturnsAsync(parentAccount);
        _accountRepository
            .Setup(a => a.GetAccountChildrenAsync(parentAccount.Id))
            .ReturnsAsync([childAccount]);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().Be(expectedCodeGroup);
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextCodeAsync))]
    public async Task CalculateNextCode_WhenTheChildCodIsAtTheLimit_ShouldReturnEmptyCodeGroup()
    {
        // Assert
        var parentAccountId = Guid.NewGuid();
        var parentAccount = new Account
        {
            Code = _faker.Random.Short(1, 999),
            Id = parentAccountId,
            IdAccountType = Guid.NewGuid(),
            Name = _faker.Lorem.Word()
        };

        var childAccount = new Account
        {
            Code = 999,
            Id = Guid.NewGuid(),
            IdAccountType = Guid.NewGuid(),
            IdParentAccount = parentAccount.Id,
            Name = _faker.Lorem.Word()
        };

        _accountRepository.Setup(a => a.GetByIdAsync(parentAccountId)).ReturnsAsync(parentAccount);
        _accountRepository
            .Setup(a => a.GetAccountChildrenAsync(parentAccount.Id))
            .ReturnsAsync([childAccount]);

        // Act
        var codeGroup = await _accountService.CalculateNextCodeAsync(parentAccountId);

        // Assert
        codeGroup.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextNewParentCodeAsync))]
    public async Task CalculateNextNewParentCode_WhenAccounHasNoChildren_ShouldReturnEmptyCode()
    {
        // Arrange
        var currentAccountId = Guid.NewGuid();

        var account = new Account { Id = currentAccountId };

        _accountRepository.Setup(a => a.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);

        // Act
        var (suggestedCode, accountResponse) = await _accountService.CalculateNextNewParentCodeAsync(Guid.NewGuid());

        // Assert
        suggestedCode.Should().BeEmpty();
    }

    [Fact]
    [Trait(nameof(AccountService), nameof(AccountService.CalculateNextNewParentCodeAsync))]
    public async Task CalculateNextNewParentCode_WhenAccounHasNoChildren_ShouldReturnCode()
    {
        // Arrange
        var currentAccountId = Guid.NewGuid();
        var parentAccountId = Guid.NewGuid();

        var currentAccount = new Account { Id = currentAccountId, IdParentAccount = parentAccountId, Code = 10 };
        var parentAccount = new Account { Id = parentAccountId, Code = 1 };

        _accountRepository.Setup(a => a.GetByIdAsync(currentAccount.Id)).ReturnsAsync(currentAccount);
        _accountRepository.Setup(a => a.GetByIdAsync(parentAccount.Id)).ReturnsAsync(parentAccount);
        // Act
        var (suggestedCode, accountResponse) = await _accountService.CalculateNextNewParentCodeAsync(currentAccountId);

        // Assert
        suggestedCode.Should().NotBeEmpty();
    }
}
