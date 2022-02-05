﻿using AutoMapper;

using MediatR;

using Tabloid.Domain.DataTransferObjects;
using Tabloid.Domain.Entities;
using Tabloid.Domain.Enums;
using Tabloid.Domain.Interfaces;
using Tabloid.Infrastructure.Repositories.Interfaces;

namespace Tabloid.Application.Commands.Albums.DeleteAlbum
{
    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, CommandResponse<AlbumDto>>
    {
        private readonly IUnitOfWork<Guid> _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAlbumCommandHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CommandResponse<AlbumDto>> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<IAlbumRepository>();
            var entity = _mapper.Map<Album>(request.Album);

            if (await repository.Contains(entity))
            {
                repository.Remove(entity);
                await _unitOfWork.Save();

                return new CommandResponse<AlbumDto>(_mapper.Map<AlbumDto>(entity));
            }

            return new CommandResponse<AlbumDto>(
                _mapper.Map<AlbumDto>(entity),
                CommandResult.Failure,
                "The album could not be found");
        }
    }
}
