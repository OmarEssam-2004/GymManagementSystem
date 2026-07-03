
using AutoMapper;
using GymManagementSystem.BLL.Coomon;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Eunms;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate)return Result.ValidationFailed("EndDate Must Be After StartDate"); 
            if (model.StartDate <= DateTime.Now)return Result.ValidationFailed("StartDate Must Be After Now"); 
            if (model.Capacity < 1 || model.Capacity > 25)return Result.ValidationFailed("Capacity Must Be Between 1 and 25"); 

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId,ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId,ct);
            if (category == null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var categorySpecialty);
            if(!isValid || trainer.Specialty != categorySpecialty) return Result.ValidationFailed("Trainer Specialty Mismatch");


           var Session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(Session);
           var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to create session");


        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (sessions is null || !sessions.Any()) return null!;

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSessions;
        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int SessionId, CancellationToken ct = default)
        {
           var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(SessionId, ct);
            if(session is null) return Result<SessionViewModel>.NotFound($"Session With Id {SessionId} Not Found");

           var mappedSession = _mapper.Map<SessionViewModel>(session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);

            return Result<SessionViewModel>.Ok(mappedSession);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync(CancellationToken ct = default)
        {
          var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
           return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(category);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId, ct);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound($"Session With Id {SessionId} Not Found");
            if(session.StartDate <= DateTime.Now) return Result<UpdateSessionViewModel>.Conflict("Cannot Update Session That Has Already Started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (bookingCount > 0) return Result<UpdateSessionViewModel>.Conflict("Cannot Update Session That Has Booked Slots");

            var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId, ct);
            if (session is null) return Result.Conflict($"Session With Id {SessionId} Not Found");
            if (session.StartDate <= DateTime.Now) return Result.Conflict("Cannot Update Session That Has Already Started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (bookingCount > 0) return Result.Conflict("Cannot Update Session That Has Booked Slots");

            if (model.StartDate >= model.EndDate) return Result.ValidationFailed("EndDate Must Be After StartDate");
            if (model.StartDate <= DateTime.Now) return Result.ValidationFailed("StartDate Must Be In The Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
            if (category == null) return Result.NotFound($"Category With Id {session.CategoryId} Not Found");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var categorySpecialty);
            if (!isValid || trainer.Specialty != categorySpecialty) return Result.ValidationFailed("Trainer Specialty Mismatch");

            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.TrainerId = model.TrainerId;
            session.Description = model.Description;
            session.UpdatedAt = DateTime.Now;


            _unitOfWork.GetRepository<Session>().Update(session);
           var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.Conflict("Failed to update session");

        }
        public async Task<Result> DeleteSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId, ct);
            if (session is null) return Result.Conflict($"Session With Id {SessionId} Not Found !");
            if(session.EndDate > DateTime.Now) return Result.Conflict("Can Not Delete Session Not End Date");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (bookingCount > 0) return Result.Conflict("Cannot Delete Session That Has Booked Slots");

            _unitOfWork.GetRepository<Session>().Delete(session);
           var count = await _unitOfWork.SaveChangesAsync(ct);
           return count > 0 ? Result.Ok() : Result.Conflict("Failed To Delete Session");    

        }
    }
}