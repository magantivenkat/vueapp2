using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Services.Email
{
    public interface IEmailSchedulingService
    {
        Task Schedule(EmailAuditBatch batch);
    }
}
