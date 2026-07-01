using AutoMapper;
using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Trainers;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            var trainersViewModel = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return trainersViewModel;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            var model = _mapper.Map<TrainerViewModel>(trainer);
            return model;
        }

        public async Task<Result> CreateTrainerAsync(TrainerToAddViewModel model, CancellationToken ct)
        {
            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return Result.ValidationFailed("Email or Phone Already Exists");

            var trainer = _mapper.Map<Trainer>(model);

            _unitOfWork.GetRepository<Trainer>().Add(trainer);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Create Trainer");
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            var model = _mapper.Map<TrainerToUpdateViewModel>(trainer);
            return model;
        }

        public async Task<Result> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Email == model.Email && T.Id != trainerId, ct);
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(T => T.Phone == model.Phone && T.Id != trainerId, ct);

            if (emailExists || phoneExists) return Result.ValidationFailed("Email or Phone Already Exists");

            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialty = model.Specialty;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);

            var count = await _unitOfWork.SaveChangesAsync(); // تم الحفاظ عليها بدون تمرير الـ ct بناءً على ملفك الأصلي
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Update Trainer");
        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var hasFutureSessions = await _unitOfWork.GetRepository<Session>().AnyAsync(S => S.TrainerId == trainerId && S.StartDate > DateTime.Now, ct);
            if (hasFutureSessions) return Result.Conflict("Trainer Cannot Be Deleted");

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var count = await _unitOfWork.SaveChangesAsync();
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to Delete Trainer");
        }

    }
}