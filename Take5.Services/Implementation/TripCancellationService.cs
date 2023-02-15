using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class TripCancellationService : ITripCancellationService
    {
        private readonly IRepository<TripCancellation, long> _repository;
        private readonly ILogger<TripCancellationService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;

        public TripCancellationService(IRepository<TripCancellation, long> repository,
            ILogger<TripCancellationService> logger,
            IMapper mapper,
            IEmployeeService employeeService,
            UserManager<AspNetUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _employeeService = employeeService;
            _userManager = userManager;
        }
        public Task<bool> CreateTripCancellation(TripCancellationModel tripCancellationModel)
        {
            try
            {
                if(tripCancellationModel != null)
                {
                    TripCancellation tripCancellation = _mapper.Map<TripCancellation>(tripCancellationModel);
                    var tripCancelled = _repository.Add(tripCancellation);
                    if(tripCancelled != null)
                    {
                        return Task<bool>.FromResult(true);
                    }
                    else
                    {
                        return Task<bool>.FromResult(false);
                    }
                }
                else
                {
                    return Task<bool>.FromResult(false);
                }
            }
            catch(Exception e)
            {
                return Task<bool>.FromResult(false);
            }
        }

        public async Task<TripCancellationModel> GetTripCancellation(long TripId)
        {
            try
            {
              TripCancellation tripCancellation = _repository.Find(TC => TC.TripId == TripId && TC.Trip.IsVisible == true).FirstOrDefault();
              TripCancellationModel tripCancellationModel = _mapper.Map<TripCancellationModel>(tripCancellation);
              if(tripCancellationModel != null)
                {
                    Employee employee = _employeeService.GetEmployeeByUserId(tripCancellation.UserId);
                    tripCancellationModel.EmployeeName = employee.EmployeeName;
                    tripCancellationModel.EmployeeNumber = employee.EmployeeNumber;
                }
                return tripCancellationModel;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
