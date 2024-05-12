using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp4.Abstractions;
using WebApp4.Abstractions.Services;
using WebApp4.DTOs;
using WebApp4.Entities.Identities;
using WebApp4.Models;

namespace WebApp4.Implementations.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;
    public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager,IUserService userService)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
    }
    public async Task<GenericResponseModel<bool>> ChangePasswordAsync(string email, string oldPassword, string newPassword)
    {
        GenericResponseModel<bool> response = new()
        {
            Data = false,
            StatusCode = 400
        };
        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(oldPassword) && string.IsNullOrEmpty(newPassword))
            return response;
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            response.StatusCode = 404;
            return response;
        }
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            response.StatusCode = 500;
            return response;
        }
        response.StatusCode = 200;
        response.Data = true;
        return response;
    }
    public async Task<GenericResponseModel<TokenDTO>> LoginAsync(string userNameOrEmail, string password)
    {
        GenericResponseModel<TokenDTO> response = new GenericResponseModel<TokenDTO>()
        {
            Data = null,
            StatusCode = 400
        };
        if (string.IsNullOrEmpty(userNameOrEmail) || string.IsNullOrEmpty(password))
        {
            return response;
        }

        var user = await _userManager.FindByNameAsync(userNameOrEmail) ?? await _userManager.FindByEmailAsync(userNameOrEmail);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            response.StatusCode = 401; // Unauthorized
            return response;
        }

        var accessTokenResult = await _tokenHandler.CreateAccessToken(user);
        await _userService.UpdateRefreshToken(accessTokenResult.RefreshToken, user, accessTokenResult.Expiration);

        if (accessTokenResult == null)
        {
            response.StatusCode = 500; // Internal server error
            return response;
        }

        response.Data = accessTokenResult;
        response.StatusCode = 200; // OK
        return response;
    }
    public async Task<GenericResponseModel<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken)
    {
        GenericResponseModel<TokenDTO> response = new GenericResponseModel<TokenDTO>()
        {
            Data = null,
            StatusCode = 400
        };
        if (!string.IsNullOrEmpty(refreshToken))
        {
            return response;
        }
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        if (user == null && user.RefreshTokenEndTime<=DateTime.UtcNow)
        {
            response.StatusCode = 401;
            return response;
        }
        //await _signInManager.SignInAsync(user, isPersistent: false);
        var accessTokenResult = await _tokenHandler.CreateAccessToken(user);
        await _userService.UpdateRefreshToken(accessTokenResult.RefreshToken, user, accessTokenResult.Expiration);

        if (accessTokenResult == null)
        {
            response.StatusCode = 500; // Internal server error
            return response;
        }

        response.Data = accessTokenResult;
        response.StatusCode = 200; // OK
        return response;
    }
    public async Task<GenericResponseModel<bool>> LogOut(string userNameOrEmail)
    {
        GenericResponseModel<bool> response = new GenericResponseModel<bool>()
        {
            Data = false,
            StatusCode = 400
        };
        if (string.IsNullOrEmpty(userNameOrEmail))
        {
            return response;
        }
        var user = await _userManager.FindByNameAsync(userNameOrEmail) ?? await _userManager.FindByEmailAsync(userNameOrEmail);
        if (user == null)
        {
            response.StatusCode = 404; // Not found
            return response;
        }
        user.RefreshToken = null;
        user.RefreshTokenEndTime = null;
        var result = await _userManager.UpdateAsync(user);
        await _signInManager.SignOutAsync();
        if(!result.Succeeded)
        {
            response.StatusCode = 500;
            return response;
        }
        response.Data = true;
        response.StatusCode = 200;
        return response;
    }
}
