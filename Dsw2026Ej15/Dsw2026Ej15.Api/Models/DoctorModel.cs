using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Api.Models
{
    public record DoctorModel
    {
        public record Request(string Name, string LicenseNumber, Guid SpecialityId);

        public record Response(string Name, string LicenseNumber, string SpecialityName)
        {
            public static Response FromEntity(Doctor doctor) =>
                new(doctor.Name, doctor.LicenseNumber, doctor.Speciality!.Name);
        }
    }
}