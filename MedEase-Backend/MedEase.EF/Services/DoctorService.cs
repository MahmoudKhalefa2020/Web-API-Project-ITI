﻿using AutoMapper;
using MedEase.Core;
using Microsoft.EntityFrameworkCore;
using MedEase.Core.Dtos;
using MedEase.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MedEase.Core.Dtos;
using MedEase.Core.Models;
using System.Reflection.Metadata;
using MedEase.Core.Consts;
using System.Net;

namespace MedEase.EF.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<Appointment> ReserveAppointmentAsync(ReserveAppointmentDto appointmentDto)
        {
            Appointment appointment = new Appointment();
            appointment = _mapper.Map<Appointment>(appointmentDto);

            await _unitOfWork.Appointments.AddAsync(appointment);
            _unitOfWork.Complete();

            return appointment;
        }
        public async Task<List<DoctorAppointmentAndPatternDto>> GetPatternAndAppointmentAsync(int Id)
        {

            List<DoctorAppointmentAndPatternDto> result = new List<DoctorAppointmentAndPatternDto>();

            IEnumerable<DoctorSchedule> drs = await _unitOfWork.DoctorSchedule.FindAllAsync(dr => dr.IsWorking == true && dr.DoctorId == Id && dr.WeekDay.Date >= DateTime.Now.Date);


            foreach (var item in drs)
            {
                IEnumerable<DateTime> reservedAppointments = (IEnumerable<DateTime>)await _unitOfWork.Appointments.FindAllWithSelectAsync(app => app.Status != Status.canceled && app.DoctorID == Id && app.Date.Date == item.WeekDay.Date, app => app.Date);
                result.Add(new DoctorAppointmentAndPatternDto() { ReservedAppointmanet = reservedAppointments.ToList(), EndTime = item.EndTime, StartTime = item.StartTime, Pattern = item.TimeInterval, WeekDay = item.WeekDay });
            }
            return result;

        }


        public async Task<List<DoctorInfoGetDto>> GetAll()
        {
            List<DoctorInfoGetDto> doctorsDTOs;
            var doctors = await _unitOfWork.Doctors.FindAllAsync(d => d.ID > 0, new List<Expression<Func<Doctor, object>>>()
            {
               d=>d.AppUser,
               d=>d.Insurances,
               d=>d.Certificates,
               d=>d.SubSpecialities,
               d=>d.Speciality,
               d=>d.AppUser.Address
            });
            doctorsDTOs = new List<DoctorInfoGetDto>();
            if (doctors != null)
            {
                if (doctors.Count() > 0)
                {

                    foreach (Doctor doctor in doctors.ToList())
                    {
                        DoctorInfoGetDto doctorDTO = new DoctorInfoGetDto();
                        doctorDTO.Faculty = doctor.Faculty;
                        doctorDTO.addressDto = doctor.AppUser.Address;
                        doctorDTO.Name = doctor.AppUser.FirstName + " " + doctor.AppUser.LastName;
                        doctorDTO.age = calucaluteAge(doctor.AppUser.BirthDate);
                        doctorDTO.ID = doctor.ID;
                        doctorDTO.Fees = doctor.Fees;
                        doctorDTO.Gender = doctor.AppUser.Gender;
                        doctorDTO.Phone = doctor.AppUser.PhoneNumber;

                        foreach (DoctorSubspeciality subspecialities in doctor.SubSpecialities)
                        {
                            SubspecialityDto subspeciality = new SubspecialityDto();
                            subspeciality.id = subspecialities.SubSpeciality.ID;
                            subspeciality.name = subspecialities.SubSpeciality.Name;

                            doctorDTO.subspecialities.Add(subspeciality);



                        }
                        foreach (Certificates Certificate in doctor.Certificates)
                        {
                            CertificateDto certificate = new CertificateDto();
                            certificate.Title = Certificate.Title;
                            certificate.Description = Certificate.Description;
                            certificate.IssueDate = Certificate.IssueDate;
                            certificate.Issuer = Certificate.Issuer;

                            doctorDTO.certificates.Add(certificate);

                        }

                        foreach (DoctorInsurance innsurances in doctor.Insurances)
                        {
                            doctorDTO.insurance.Add(innsurances.Insurance);


                        }

                        doctorDTO.SpecialityName = doctor.Speciality.Name;




                        doctorsDTOs.Add(doctorDTO);
                    }

                }

            }


            return doctorsDTOs;
        }
       

        public async Task<DoctorSchedule> CreateScheduleAsync(DoctorScheduleDto scheduleDto)
        {
            if (!scheduleDto.IsWorking)
            {
                DoctorSchedule schedule = new DoctorSchedule();
                schedule = _mapper.Map<DoctorSchedule>(scheduleDto);

                await _unitOfWork.DoctorSchedules.AddAsync(schedule);
                _unitOfWork.Complete();

                return schedule;

            }

            return null; 




        }
        public async Task<List<DoctorInsurance>> GetDoctorInsurranecs(int id)
        {
            throw new NotImplementedException();
        }

        public int calucaluteAge(DateTime birtdate)
        {
            DateTime dataNow = DateTime.Today;

            return dataNow.Year - birtdate.Year;
        }
        
    }
}
