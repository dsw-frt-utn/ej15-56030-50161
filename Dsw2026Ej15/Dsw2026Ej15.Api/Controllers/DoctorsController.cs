using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

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
            {
                throw new ValidationException("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("La matrícula es requerida");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality is null)
            {
                throw new ValidationException("La especialidad no existe");
            }

            var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
            _persistence.SaveDoctor(doctor);

            return Created();
        }

        [HttpGet("doctors")]
        public IActionResult GetActiveDoctors()
        {
            var doctors = _persistence.GetActiveDoctors()
                .Select(d => new DoctorModel.Response(d.Name, d.LicenseNumber, d.Speciality!.Name));

            return Ok(doctors);
        }

        [HttpGet("doctors/{id}")]
        public IActionResult GetActiveDoctorById(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);

            if (doctor is null)
            {
                return NotFound();
            }

            var response = new DoctorModel.Response(doctor.Name, doctor.LicenseNumber, doctor.Speciality!.Name);
            return Ok(response);
        }

        [HttpDelete("doctors/{id}")]
        public IActionResult DeactivateDoctor(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);

            if (doctor is null)
            {
                return NotFound();
            }

            _persistence.DeactivateDoctor(id);
            return NoContent();
        }
    }
}
