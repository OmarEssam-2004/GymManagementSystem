
using AutoMapper;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagementSystem.BLL.ViewModels.Trainers;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using System;

namespace GymManagementSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ==========================================
            // 1. Members Mappings

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(D => D.Address, O => O.MapFrom(S => new Address
                {
                    BuildingNumber = S.BuildingNumber,
                    Street = S.Street,
                    City = S.City,
                }))
                .ForMember(D => D.HealthRecord, O => O.MapFrom(S => new HealthRecord
                {
                    Height = S.HealthRecordViewModel.Height,
                    Weight = S.HealthRecordViewModel.Weight,
                    BloodType = S.HealthRecordViewModel.BloodType,
                    Note = S.HealthRecordViewModel.Note,
                }));

            CreateMap<Member, MemberViewModel>()
                .ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.DateOfBirth.ToString()))
                .ForMember(D => D.Address, O => O.MapFrom(S => $"{S.Address.BuildingNumber} - {S.Address.Street} - {S.Address.City}"));

            CreateMap<HealthRecord, HealthRecordViewModel>();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(D => D.City, O => O.MapFrom(S => S.Address.City))
                .ForMember(D => D.BuildingNumber, O => O.MapFrom(S => S.Address.BuildingNumber))
                .ForMember(D => D.Street, O => O.MapFrom(S => S.Address.Street));

            // ==========================================
            // 2. Plans Mappings

            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, PlanToUpdateViewModel>();

            // ==========================================
            // 3. Sessions Mappings

            CreateMap<Session, SessionViewModel>()
                .ForMember(D => D.TrainerName, O => O.MapFrom(S => S.Trainer.Name))
                .ForMember(D => D.CategoryName, O => O.MapFrom(S => S.Category.CategoryName));
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<CreateSessionViewModel, Session>();



            // ==========================================
            // 4. Trainers Mappings

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(D => D.Gender, O => O.MapFrom(S => S.Gender.ToString()))
                .ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.DateOfBirth.ToString()))
                .ForMember(D => D.Address, O => O.MapFrom(S => $"{S.Address.BuildingNumber} - {S.Address.Street} - {S.Address.City}"));

            CreateMap<TrainerToAddViewModel, Trainer>()
                .ForMember(D => D.Address, O => O.MapFrom(S => new Address
                {
                    BuildingNumber = S.BuildingNumber,
                    City = S.City,
                    Street = S.Street
                }));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(D => D.City, O => O.MapFrom(S => S.Address.City))
                .ForMember(D => D.BuildingNumber, O => O.MapFrom(S => S.Address.BuildingNumber))
                .ForMember(D => D.Street, O => O.MapFrom(S => S.Address.Street));


        }
    }
}