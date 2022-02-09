﻿using MediatR;

using Tabloid.Domain.DataTransferObjects;

namespace Tabloid.Application.Commands.Genres.DeleteGenre
{
    public class DeleteGenreCommand : IRequest<CommandResponse<GenreDto>>
    {
        public DeleteGenreCommand(GenreDto genre)
        {
            Genre = genre;
        }

        public GenreDto Genre { get; set; }
    }
}