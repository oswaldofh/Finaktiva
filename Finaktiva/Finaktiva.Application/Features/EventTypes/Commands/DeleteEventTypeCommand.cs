﻿using Finaktiva.Application.Abstractions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventTypes.Commands
{
    public class DeleteEventTypeCommand : IRequest<Response<bool>>
    {
        [Required]
        public int Id { get; set; }
        public DeleteEventTypeCommand(int id) => Id = id;
    }
}
