using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Users.DTOs;
using ChatApp.Domain;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Handlers.Users.Queries
{
    public class GetAllUsersQuery : IRequest<CustomeResponse<PageResult<DTO_GetAllUsersQuery>>>
    {
        public string CurrentUserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, CustomeResponse<PageResult<DTO_GetAllUsersQuery>>>
    {
        private readonly IAppUserRepository<AppUser> _appUserRepo;

        public GetAllUsersQueryHandler(IAppUserRepository<AppUser> appUserRepo)
        {
            this._appUserRepo = appUserRepo;
        }

        public async Task<CustomeResponse<PageResult<DTO_GetAllUsersQuery>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // 1️⃣ Get paginated AppUsers
            var usersPage = await _appUserRepo.GetPaginatedAsync(
                request.PageNumber,
                request.PageSize,
                a => a.Id != request.CurrentUserId
            );

            // 2️⃣ Map entities to DTOs
            var dtoItems = usersPage.Items
                .Select(a => new DTO_GetAllUsersQuery
                {
                    Id = a.Id,
                    Name = a.UserName,
                    ImagePath = a.ImagePath,
                    IsOnline = a.IsOnline
                }).ToList();
                

            // 3️⃣ Create a new PageResult with DTO type
            var dtoPage = new PageResult<DTO_GetAllUsersQuery>
            {
                Data = dtoItems,
                TotalCount = usersPage.TotalCount,
                PageNumber = usersPage.PageNumber,
                PageSize = usersPage.PageSize,
            };

            // 4️⃣ Return response
            return dtoPage.Data.Any() ? CustomeResponse<PageResult<DTO_GetAllUsersQuery>>.Success(dtoPage)
                                       : CustomeResponse<PageResult<DTO_GetAllUsersQuery>>.Fail("No Users Found!");

        }
    }
}
