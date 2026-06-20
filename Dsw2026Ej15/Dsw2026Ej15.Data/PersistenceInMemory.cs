using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private List<Speciality> _specialities = [];
        private List<Doctor> _doctors = [];

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        public void LoadSpecialities()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");

                var json = File.ReadAllText(jsonPath);
                var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>
                    (json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
                _specialities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];
            }
            catch (Exception)
            {
            }

        }

        public Speciality? GetSpecialityById(Guid id)
        {
            return _specialities.SingleOrDefault(e => e.Id == id);
        }

        public void SaveDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

        public IEnumerable<Doctor> GetActiveDoctors()
        {
            return _doctors.Where(d => d.IsActive);
        }

        public Doctor? GetActiveDoctorById(Guid id)
        {
            return _doctors.SingleOrDefault(d => d.Id == id && d.IsActive);
        }

        public void DeactivateDoctor(Guid id)
        {
            var doctor = _doctors.Single(d => d.Id == id);
            doctor.Deactivate();
        }

    }
