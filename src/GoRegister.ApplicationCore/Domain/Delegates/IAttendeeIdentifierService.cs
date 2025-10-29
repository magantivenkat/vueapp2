using GoRegister.ApplicationCore.Data;
using System;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public interface IAttendeeIdentifierService
    {
        string CreateAttendeeNumber();
        string CreateAttendeeNumberForBulkUpload(string bulkInsertion);
    }

    public class AttendeeIdentifierService : IAttendeeIdentifierService
    {
        private const string CHARACTERSET = "BCDFGHJKLMNPQRSTVWXYZ1234567890";
        private const int CHARACTERSET_LENGTH = 31;
        private const string SINGLE_CHARACTERSET = "NPQRSTVWXYZ12345";
        private const int SINGLE_CHARACTERSET_LENGTH = 11;
        private const string BULK_CHARACTERSET = "BCDFGHJKLM67890";
        private const int BULK_CHARACTERSET_LENGTH = 11;
        private const int IDENTIFIER_LENGTH = 6;

        private readonly ApplicationDbContext _db;

        public AttendeeIdentifierService(ApplicationDbContext db)
        {
            _db = db;
        }

        public string CreateAttendeeNumber()
        {
            while (true)
            {
                var number = GenerateRandomString(CHARACTERSET, CHARACTERSET_LENGTH, IDENTIFIER_LENGTH);

                if (_db.Delegates.Any(d => d.AttendeeNumber == number)) continue;
                return number;
            }
        }

        public string CreateAttendeeNumberForBulkUpload(string bulkInsertion)
        {
            return GenerateRandomString(CHARACTERSET, CHARACTERSET_LENGTH, IDENTIFIER_LENGTH);
        }

        private string GenerateRandomString(string characters, int characterLength, int length)
        {
            var randomStringChars = new char[length];
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomStringChars[i] = characters[random.Next(characterLength - 1)];
            }

            return new string(randomStringChars);
        }
    }
}
