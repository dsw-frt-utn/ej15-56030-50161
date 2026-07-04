using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private readonly List<Speciality> _specialities = [];
        private readonly List<Doctor> _doctors = [];
        public PersistenceInMemory()
        {
            InitializeData();
        }

        public void InitializeData()
        {
            InitializeSpecialities();
            InitializeDoctors();
        }

        private void InitializeSpecialities()
        {
            var especialidadesData = LoadSpecialities<SpecialityDto>("specialities");
            if (especialidadesData != null)
            {
                foreach (var data in especialidadesData)
                {
                    Speciality s = new(data.Name, data.Description, data.Id);
                    _specialities.Add(s);
                }
            }
        }

        private void InitializeDoctors()
        {
            var s1 = _specialities.Find(s => s.Name == "Traumatología");
            var s2 = _specialities.Find(s => s.Name == "Oftalmología");
            var s3 = _specialities.Find(s => s.Name == "Cardiología");
            var s4 = _specialities.Find(s => s.Name == "Dermatología");

            Doctor d1 = new("Manuel Peralta", "5296", s1!, Guid.Parse("4ab58e4f-4ad5-7bf5-75e5-5bf42a3f4bf1"));
            Doctor d2 = new("Ramon Mortadela", "4851", s2!, Guid.Parse("5f68dae2-e7ff-f4a5-9fd1-f4a5ba45b31f"));
            Doctor d3 = new("Ricardo Cruz", "6982", s3!, Guid.Parse("ab58a6a2-f4a5-f7a5-a81f-7f4b2ad5f637"));
            Doctor d4 = new("Samuel Ramos", "1475", s4!, Guid.Parse("a4b5afb4-f5be-d7bc-77dc-f4aa223fb6ba"));

            _doctors.Add(d1);
            _doctors.Add(d2);
            _doctors.Add(d3);
            _doctors.Add(d4);
        }

        private List<T>? LoadSpecialities<T>(string file)
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", $"{file}.json");
            string jsonContent = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<T>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return _doctors.Where(d => d.IsActive);
        }
        public async Task<Doctor?> GetDoctorById(Guid id)
        {
            var doctor = _doctors.Find(d => d.Id == id);
            if (doctor == null || doctor.IsActive == false)
            {
                return null;
            }
            return doctor;
        }

        public async Task<Speciality?> GetSpecialityById(Guid id)
        {
            return _specialities.Find(s => s.Id == id);
        }

        public async Task SaveDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

        public async Task UpdateDoctor(Doctor doctor)
        {
            var index = _doctors.FindIndex(d => d.Id == doctor.Id);
            if (index >= 0)
            {
                _doctors[index] = doctor;
            }
        }
    }
}
