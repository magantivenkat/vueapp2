using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.RegistrationPaths
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public Query(int id)
            {
                Id = id;
            }

            public Query(Command command)
            {
                CommandToPopulate = command;
            }

            public int? Id { get; set; }
            public Command CommandToPopulate { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query message, CancellationToken cancellationToken)
            {
                var command = message.CommandToPopulate;
                if (command == null)
                {
                    if (message.Id == null)
                    {
                        command = new Command();
                    }
                    else
                    {
                        command = await _db.RegistrationPaths
                            .Where(c => c.Id == message.Id)
                            .ProjectTo<Command>(_mapper.ConfigurationProvider)
                            .SingleOrDefaultAsync();
                    }
                }

                if (command != null) PopulateViewData(command);

                return command;
            }

            public void PopulateViewData(Command command)
            {

            }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public CommandHandler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                RegistrationPath path;
                if (request.Id == 0)
                {
                    path = new RegistrationPath();
                    await _db.RegistrationPaths.AddAsync(path);
                }
                else
                {
                    path = await _db.RegistrationPaths.FindAsync(request.Id);
                }

                _mapper.Map(request, path);
                await _db.SaveChangesAsync();
                return path.Id;
            }
        }

        public class Command : IRequest<int>
        {
            public int Id { get; set; }

            [DisplayName("Is active")]
            public bool IsActive { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            
            [DisplayName("Date registration from")]
            public DateTime? DateRegistrationFrom { get; set; }

            [DisplayName("Date registration to")]
            public DateTime? DateRegistrationTo { get; set; }

            [DisplayName("Date modify to")]
            public DateTime? DateModifyTo { get; set; }

            [DisplayName("Date cancel to")]
            public DateTime? DateCancelTo { get; set; }

            [DisplayName("Can cancel")]
            public bool CanCancel { get; set; }

            [DisplayName("Can modify")]
            public bool CanModify { get; set; }

            [DisplayName("Date created")]
            public DateTime DateCreated { get; set; }

            [DisplayName("Not invited text")]
            public string NotInvitedText { get; set; }

            [DisplayName("Invited text")]
            public string InvitedText { get; set; }

            [DisplayName("Confirmed text")]
            public string ConfirmedText { get; set; }

            [DisplayName("Declined text")]
            public string DeclinedText { get; set; }

            [DisplayName("Cancelled text")]
            public string CancelledText { get; set; }

            [DisplayName("Waiting text")]
            public string WaitingText { get; set; }
        }
    }
}
