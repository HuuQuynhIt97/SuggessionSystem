using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Suggession.Data;
using Suggession.DTO.auth;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Suggession._Repositories.Interface;

namespace Suggession.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IAccountGroupAccountRepository _repoAcgA;

        private readonly IAccountGroupRepository _repoAcg;
        public AuthController(
            IConfiguration config,
            IMapper mapper,
            IAccountGroupAccountRepository repoAcgA,
            IAccountGroupRepository repoAcg,
            IAuthService authService
            )
        {
            _config = config;
            _mapper = mapper;
            _repoAcgA = repoAcgA;
            _repoAcg = repoAcg;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _authService.Login(userForLoginDto.Username, userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var isLock= await _authService.CheckLock(userForLoginDto.Username);
            if (isLock)
                return Unauthorized("The account has been locked!");
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userTamp = new UserForDetailDto()
            {
                Id = userFromRepo.Id.ToString(),
                FullName = userFromRepo.FullName,
                Username = userFromRepo.Username,
                AccountGroupId = _repoAcgA.FindAll(y => y.AccountId == userFromRepo.Id).ToList().Count > 0 ?
                _repoAcgA.FindAll(y => y.AccountId == userFromRepo.Id).FirstOrDefault().AccountGroupId : 0
            };

            var user = new UserForDetailDto()
            {
                Id = userTamp.Id,
                FullName = userTamp.FullName,
                Username = userTamp.Username,
                AccountGroupText = _repoAcg.FindById(userTamp.AccountGroupId) != null ?
                _repoAcg.FindById(userTamp.AccountGroupId).Name : null,
                AccountGroupSequence = _repoAcg.FindById(userTamp.AccountGroupId) != null ?
                _repoAcg.FindById(userTamp.AccountGroupId).Sequence : null
            };
            //_mapper.Map<UserForDetailDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }

    }
}
