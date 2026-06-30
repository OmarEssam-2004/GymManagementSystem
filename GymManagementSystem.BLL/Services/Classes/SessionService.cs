
using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Eunms;
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

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate)return false; 
            if (model.StartDate <= DateTime.Now)return false; 
            if (model.Capacity < 1 || model.Capacity > 25)return false; 

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId,ct);
            if (trainer == null) return false;

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId,ct);
            if (category == null) return false;

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var categorySpecialty);
            if(!isValid || trainer.Specialty != categorySpecialty) return false; 

           var Session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(Session);
           var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;


        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (sessions is null || !sessions.Any()) return null!;

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions).ToList();

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSessions;
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

    }
}