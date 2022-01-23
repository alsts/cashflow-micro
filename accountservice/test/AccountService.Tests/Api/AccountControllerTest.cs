using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AccountService.Data;
using AccountService.Dtos;
using AccountService.Tests.Common;
using AccountService.Util.Enums;
using AccountService.Util.Helpers;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace AccountService.Tests.Api
{
    public class UsersControllerIntegrationTest : IntegrationTestBase<AppDbContext, Startup>
    {
        [Fact]
        public async Task SignIn_with_non_existing_user_should_return_NotFound()
        {
            // Arrange
            var formModel = new Dictionary<string, string>
            {
                { "username", "notexistinguser" },
                { "password", "password123!" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignIn, formModel);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SignIn_with_existing_user_with_incorrect_password_should_return_NotFound()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            var formModel = new Dictionary<string, string>
            {
                { "username", "user" },
                { "password", "notcorrectpassword" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignIn, formModel);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SignIn_with_existing_user_with_correct_password_should_return_Ok_WithUserReadDto()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            var formModel = new Dictionary<string, string>
            {
                { "username", "user" },
                { "password", "password" },
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignIn, formModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var userResponse = await response.Content.ReadAsAsync<UserReadDto>();
            userResponse.Should().NotBeNull();

            // Check Cookies
            response.Headers.TryGetValues(HeaderNames.SetCookie, out IEnumerable<string> cookies);
            Assert.NotNull(cookies);
            Assert.NotEmpty(cookies);

            // Check Auth Cookie
            var cookiesList = cookies.ToList();
            Assert.Contains("X-Access-Token", cookiesList[0]);
        }

        [Theory, MemberData(nameof(GetInvalidSignUpDtos))]
        public async Task SignUp_with_incorrect_user_data_should_return_InternalError(Dictionary<string, string> signUpDto, string expectedError)
        {
            // Arrange
            var formModel = new Dictionary<string, string>
            {
                { "username", signUpDto["username"] },
                { "email", signUpDto["email"] },
                { "password", signUpDto["password"] },
                { "firstname", "Firstname" },
                { "lastname", "Lastname" },
                { "gender", "1" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains(expectedError, userResponse);
        }

        [Fact]
        public async Task SignUp_with_existing_users_data_should_return_BadRequest_UserAlreadyExists()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Username is duplicated
            var formModel = new Dictionary<string, string>
            {
                { "username", "user" },
                { "email", "user123@cashflow.com" },
                { "password", "parolaA123!" },
                { "firstname", "User" },
                { "lastname", "User" },
                { "gender", "1" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("Username is already taken", userResponse);

            // Make username unique, email duplicated
            formModel["username"] = "user123";
            formModel["email"] = "user@cashflow.com";

            // Act
            response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("Email is already taken", userResponse);
        }

        [Fact]
        public async Task SignUp_with_blank_required_fields_should_return_BadRequest_UserAlreadyExists()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Blank username
            var formModel = new Dictionary<string, string>
            {
                { "username", "" },
                { "email", "user123@cashflow.com" },
                { "password", "parolaA123!" },
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Username field is required", userResponse);

            // Blank email
            formModel["username"] = "testuser";
            formModel["email"] = "";

            // Act
            response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Email field is required", userResponse);

            // Blank password
            formModel["username"] = "testuser";
            formModel["email"] = "testuser@cashflow.com";
            formModel["password"] = "";

            // Act
            response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Password field is required", userResponse);
        }

        [Fact]
        public async Task SignUp_with_correct_data_should_return_Ok_WithUserReadDto()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // required fields
            var formModel = new Dictionary<string, string>
            {
                { "username", "usernew" },
                { "email", "user123@cashflow.com" },
                { "password", "parolaA123!" }
            };

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignUp, formModel);

            // Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var userResponse = await response.Content.ReadAsAsync<UserReadDto>();
            userResponse.Should().NotBeNull();

            // Check Cookies
            response.Headers.TryGetValues(HeaderNames.SetCookie, out IEnumerable<string> cookies);
            Assert.NotNull(cookies);
            Assert.NotEmpty(cookies);

            // Check Auth Cookie
            var cookiesList = cookies.ToList();
            Assert.Contains("X-Access-Token", cookiesList[0]);
        }
        
        [Fact]
        public async Task GetCurrent_withoutToken_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetCurrent);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCurrent_with_basicUserToken_return_Ok_UserReadDto()
        {
            // Arrange
            var user = CreateUser("user123");
            Arrange(dbContext =>
            {
                dbContext.Users.Add(user);
                dbContext.Users.Add(CreateUser("user234"));
            });

            // Act
            await AuthenticateAsync("user123");
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetCurrent);

            // Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var userResponse = await response.Content.ReadAsAsync<UserReadDto>();
            userResponse.Should().NotBeNull();
            userResponse.Should().BeEquivalentTo(user.ToPublicDto());
        }
        
        [Fact]
        public async Task RefreshToken_withoutToken_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Account.Refresh);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_with_basicUserToken_return_Ok_UserReadDto()
        {
            // Arrange
            var user = CreateUser("user123");
            Arrange(dbContext =>
            {
                dbContext.Users.Add(user);
                dbContext.Users.Add(CreateUser("user234"));
            });

            // Act
            await AuthenticateAsync("user123");
            var response = await TestClient.GetAsync(ApiRoutes.Account.Refresh);

            // Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_withoutToken_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });
            
            var formModel = new Dictionary<string, string>
            {
                { "email", "user123@cashflow.com" },
                { "firstname", "User" },
                { "lastname", "User" },
            };

            // Act
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task Update_with_blank_required_fields_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });
            
            // Blank email
            var formModel = new Dictionary<string, string>
            {
                { "email", "" },
                { "firstname", "User" },
                { "lastname", "User" },
            };

            // Act
            await AuthenticateAsync("user");
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Email field is required", userResponse);
            
            // Blank firstname
            formModel["email"] = "user@cashflow.com";
            formModel["firstname"] = "";
            
            // Act
            await AuthenticateAsync("user");
            response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Firstname field is required", userResponse);
            
            // Blank lastname
            formModel["firstname"] = "user";
            formModel["lastname"] = "";
            
            // Act
            await AuthenticateAsync("user");
            response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("The Lastname field is required", userResponse);
        }
        
        [Fact]
        public async Task Update_with_existing_email_return_BadRequest_emailAlreadyTaken()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user123"),
                    CreateUser("user456")
                );
            });
            
            var formModel = new Dictionary<string, string>
            {
                { "email", "user456@cashflow.com"},
                { "firstname", "User" },
                { "lastname", "User" },
            };

            // Act
            await AuthenticateAsync("user123");
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var userResponse = await response.Content.ReadAsStringAsync();
            userResponse.Should().NotBeNull();
            Assert.Contains("Email is already taken", userResponse);
        }
        
        [Fact]
        public async Task Update_with_correctData_return_Ok_UserUpdated()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user123"),
                    CreateUser("user456")
                );
            });
            
            var formModel = new Dictionary<string, string>
            {
                { "email", "userupdatedemail@cashflow.com"},
                { "firstname", "UpdatedFirstName" },
                { "lastname", "UpdatedLastName" },
            };

            // Act
            await AuthenticateAsync("user123");
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Account.Update, formModel);

            // Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var userResponse = await response.Content.ReadAsAsync<UserReadDto>();
            Assert.Equal(formModel["email"],userResponse.Email);
            Assert.Equal(formModel["firstname"],userResponse.Firstname);
            Assert.Equal(formModel["lastname"],userResponse.Lastname);
        }
        
        [Fact]
        public async Task GetById_withoutToken_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetById.Replace("{id}", ""));

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetById_with_basicUserToken_return_NotFound()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
            });

            // Act
            await AuthenticateAsync("user");
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetById.Replace("{id}", "NonExistingUserId"));

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_with_basicUserToken_return_Ok_UserReadDto()
        {
            // Arrange
            var userToFind = CreateUser("user123");
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("user")
                );
                dbContext.Users.Add(userToFind);
            });

            // Act
            await AuthenticateAsync("user");
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetById.Replace("{id}", userToFind.PublicId));

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userResponse = await response.Content.ReadAsAsync<UserReadDto>();
            userResponse.Should().NotBeNull();
            userResponse.Should().BeEquivalentTo(userToFind.ToPublicDto());
        }
        
        [Fact]
        public async Task GetAll_withoutToken_return_Unauthorized()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("admin", Roles.Admin),
                    CreateUser("user")
                );
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetAll);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_with_basicUserToken_return_Forbidden()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("admin", Roles.Admin),
                    CreateUser("user")
                );
            });

            // Act
            await AuthenticateAsync("user");
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetAll);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAll_with_adminUserToken_return_Ok_and_collection_of_users()
        {
            // Arrange
            Arrange(dbContext =>
            {
                dbContext.Users.AddRange(
                    CreateUser("admin", Roles.Admin),
                    CreateUser("user")
                );
            });

            // Act
            await AuthenticateAsync("admin");
            var response = await TestClient.GetAsync(ApiRoutes.Account.GetAll);

            // Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<UserReadDto>>()).Should().NotBeEmpty();
        }

        public static IEnumerable<object[]> GetInvalidSignUpDtos
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new Dictionary<string, string>
                        {
                            { "username", "u" },
                            { "email", "user@cashflow.com" },
                            { "password", "parolaA123!" }
                        },
                        "Username does not match requirements"
                    },
                    new object[]
                    {
                        new Dictionary<string, string>
                        {
                            { "username", "user" },
                            { "email", "user@cashflow.com" },
                            { "password", "password" }
                        },
                        "Password does not match requirements"
                    },
                    new object[]
                    {
                        new Dictionary<string, string>
                        {
                            { "username", "user" },
                            { "email", "useremail" },
                            { "password", "parolaA123!" }
                        },
                        "Need to specify valid email"
                    }
                };
            }
        }
    }
}
