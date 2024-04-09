using LegacyApp;

namespace LegacyAppTest;

public class AddUserTests
{
   [Fact]
    public void AddUser_ShouldReturnFalse_WhenFirstNameIsMissing()
    {
        // Arrange
        var clientId = 1;
        var firstName = (string)null;
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var userService = new UserService();

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ShouldReturnFalse_WhenLastNameIsMissing()
    {
        // Arrange
        var clientId = 1;
        var firstName = "John";
        var lastName = (string)null;
        var email = "john.doe@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var userService = new UserService();

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ShouldReturnFalse_WhenEmailIsInvalid()
    {
        // Arrange
        var clientId = 1;
        var firstName = "John";
        var lastName = "Doe";
        var email = "not-an-email";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var userService = new UserService();

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ShouldReturnFalse_WhenUnderage()
    {
        // Arrange
        var clientId = 1;
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = DateTime.Now.AddYears(-20);
        var userService = new UserService();

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.False(result);
    }

}