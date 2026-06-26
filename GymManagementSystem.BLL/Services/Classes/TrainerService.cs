using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Trainers;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<Session> _sessionRepository;

        public TrainerService(
            IGenericRepository<Trainer> trainerRepository,
            IGenericRepository<Session> sessionRepository)
        {
            _trainerRepository = trainerRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {
            var trainers = await _trainerRepository.GetAllAsync(ct: ct);

            var trainersViewModel = trainers.Select(trainer => new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Phone = trainer.Phone,
                Email = trainer.Email,
                Gender = trainer.Gender.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                Specialty = trainer.Specialty
            });

            return trainersViewModel;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            var model = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Gender = trainer.Gender.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                Specialty = trainer.Specialty
            };

            return model;
        }

        public async Task<bool> CreateTrainerAsync(TrainerToAddViewModel model, CancellationToken ct)
        {
            var emailExists = await _trainerRepository.AnyAsync(T => T.Email == model.Email, ct);
            var phoneExists = await _trainerRepository.AnyAsync(T => T.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return false;

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Specialty = model.Specialty,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                }
            };

            var count = await _trainerRepository.AddAsync(trainer, ct);
            return count > 0;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            var model = new TrainerToUpdateViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                City = trainer.Address.City,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                Specialty = trainer.Specialty
            };

            return model;
        }

        public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            var emailExists = await _trainerRepository.AnyAsync(T => T.Email == model.Email && T.Id != trainerId, ct);
            var phoneExists = await _trainerRepository.AnyAsync(T => T.Phone == model.Phone && T.Id != trainerId, ct);

            if (emailExists || phoneExists) return false;

            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialty = model.Specialty;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;

            var count = await _trainerRepository.UpdateAsync(trainer, ct);
            return count > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            var hasFutureSessions = await _sessionRepository.AnyAsync(S => S.TrainerId == trainerId && S.StartDate > DateTime.Now, ct);
            if (hasFutureSessions) return false;

            var count = await _trainerRepository.DeleteAsync(trainer, ct);
            return count > 0;
        }
    }
}