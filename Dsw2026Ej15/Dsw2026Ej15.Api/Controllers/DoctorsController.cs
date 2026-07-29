using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers
{
    public class DoctorsController : AppController
    {
        private readonly IPersistence _persistence;

        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost("doctors")]
        public IActionResult CreateDoctor(DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                throw new ValidationException("La matrícula es requerida");

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality is null)
                throw new ValidationException("La especialidad no existe");

            var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
            _persistence.SaveDoctor(doctor);

            return CreatedAtAction(nameof(ReadDoctorById), new { id = doctor.Id }, null);
        }

        [HttpGet("doctors")]
        public IActionResult ReadActiveDoctors()
        {
            var response = _persistence.GetActiveDoctors()
                .Select(DoctorModel.Response.FromEntity);

            return Ok(response);
        }

        [HttpGet("doctors/{id}")]
        public IActionResult ReadDoctorById(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);
            if (doctor is null)
                throw new ValidationException("No se encuentra el médico o no está activo");

            return Ok(DoctorModel.Response.FromEntity(doctor));
        }

        [HttpDelete("doctors/{id}")]
        public IActionResult DeactivateDoctor(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);
            if (doctor is null)
                throw new ValidationException("No se encuentra el médico o no está activo");

            _persistence.DeactivateDoctor(id);
            return NoContent();
        }
    }
}