using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver, long> _repository;
        private readonly ILogger<DriverService> _logger;
        private readonly IMapper _mapper;

        public DriverService(IRepository<Driver, long> repository,
          ILogger<DriverService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateDriver(DriverModel model)
        {
            try
            {
                model.IsVisible = true;
                model.IsDelted = false;
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                var driver = _mapper.Map<Driver>(model);
                var result = _repository.Add(driver);

                if (result != null)
                {
                    return Task<bool>.FromResult<bool>(true);
                }
                else
                {
                    return Task<bool>.FromResult<bool>(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return Task<bool>.FromResult<bool>(false);

            }
        }

        public bool DeleteDriver(long id)
        {
            try
            {
                var truck = _repository.Find(t => t.Id == id).FirstOrDefault();
                if (truck != null)
                {
                    truck.IsDelted = true;
                    truck.IsVisible = false;
                    bool result = _repository.Update(truck);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

        public List<DriverModel> GetAllDrivers()
        {
            try
            {
                var drivers = _repository.Findlist().Result;
                var models = new List<DriverModel>();
                models = _mapper.Map<List<DriverModel>>(drivers);
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<DriverModel> GetAllActiveDrivers()
        {
            try
            {
                var drivers = _repository.Find(d=>d.IsVisible == true).ToList();
                var models = new List<DriverModel>();
                models = _mapper.Map<List<DriverModel>>(drivers);
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public DriverModel GetDriver(long id)
        {
            try
            {
                Driver driver = _repository.Find(d => d.IsVisible == true && d.Id == id).First();
                DriverModel driverModel = _mapper.Map<DriverModel>(driver);
                return driverModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateDriver(DriverModel model)
        {
            var driver = _mapper.Map<Driver>(model);

            try
            {
                bool result = _repository.Update(driver);

                return Task<bool>.FromResult<bool>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }
        public Driver GetDriverByUserId(string userId)
        {
            try
            {
                Driver driver = _repository.Find(d => d.IsVisible == true && d.UserId == userId).FirstOrDefault();
                return driver;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DriverModel GetDriverModelByUserId(string userId)
        {
            try
            {
                Driver driver = _repository.Find(d => d.IsVisible == true && d.UserId == userId).FirstOrDefault();
                DriverModel driverModel = _mapper.Map<DriverModel>(driver);
                return driverModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<DriverAPIModel> GetAllDriversForMobile()
        {
            try
            {
                var drivers = _repository.Find(d => d.IsVisible == true).ToList();
                var models = new List<DriverAPIModel>();
                models = _mapper.Map<List<DriverAPIModel>>(drivers);
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
    }
}
