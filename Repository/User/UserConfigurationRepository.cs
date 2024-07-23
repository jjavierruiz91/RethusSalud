using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;
using System.Net;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.Constants.User.UserConfiguration;

namespace rethus_backend.Repository
{
    public class UserConfigurationRepository
        : Repository<Configurations>,
            IUserConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserConfigurationRepository(ApplicationDbContext db)
            : base(db)
        {
            _context = db;
        }

        public Configurations GetById(string id)
        {
            return _context.Configurations.Find(id);
        }

        public Configurations GetByUserId(string userId)
        {
            return _context.Configurations.FirstOrDefault(user => user.UserId == userId);
        }

        public bool IsUniqueUser(string userId)
        {
            throw new NotImplementedException();
        }

        public void Register(string email)
        {
            var user = this._context.Users.FirstOrDefault(user => user.email == email);

            Configurations newConfiguration =
                new()
                {
                    State = ConfigurationsState.Initial,
                    Step = ConfigurationStep.acept_terms_conditions,
                    TypeProcedure = ConfigurationTypeProcedure.DEFAULT,
                    TermCondition = false,
                    UserId = user.UserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

            _context.Configurations.Add(newConfiguration);
            _context.SaveChanges();
        }

        public ApiResponse updateStepConfiguration(string id, ConfigurationStep step)
        {
            var response = new ApiResponse();
            var user_configuration = GetById(id);

            if (user_configuration == null)
            {
                response.AddError(
                    "El usuario no tiene configuracion asignada",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            var nextStep = UserConfiguration.GetNextStepOnboarding(step);
            user_configuration.Step = nextStep;

            _context.Configurations.Update(user_configuration);
            _context.SaveChanges();

            response.Result = user_configuration.Step;
            return response;
        }

        public ApiResponse updateAutomaticStepConfiguration(string id)
        {
            var response = new ApiResponse();
            var user_configuration = GetById(id);

            if (user_configuration == null)
            {
                response.AddError(
                    "El usuario no tiene configuracion asignada",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            var nextStep = UserConfiguration.GetNextStepOnboarding(user_configuration.Step);
            user_configuration.Step = nextStep;

            if (nextStep == ConfigurationStep.success)
            {
                user_configuration.State = ConfigurationsState.PendingReview;
            }

            if (nextStep == ConfigurationStep.error)
            {
                user_configuration.State = ConfigurationsState.Rejected;
            }

            _context.Configurations.Update(user_configuration);
            _context.SaveChanges();

            response.Result = user_configuration.Step;
            return response;
        }

        public ApiResponse updateTypeProcessConfiguration(
            string id,
            ConfigurationTypeProcedure type_procedure
        )
        {
            var response = new ApiResponse();
            var user_configuration = GetById(id);

            if (user_configuration == null)
            {
                response.AddError(
                    "El usuario no tiene configuracion asignada",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            var nextStep = UserConfiguration.GetNextStepOnboarding(user_configuration.Step);

            user_configuration.TypeProcedure = type_procedure;
            user_configuration.Step = nextStep;

            _context.Configurations.Update(user_configuration);
            _context.SaveChanges();

            response.Result = user_configuration.TypeProcedure;
            return response;
        }

        public ApiResponse updateTermConditionsConfiguration(string id, bool term)
        {
            var response = new ApiResponse();
            var user_configuration = GetById(id);

            if (user_configuration == null)
            {
                response.AddError(
                    "El usuario no tiene configuracion asignada",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            var nextStep = UserConfiguration.GetNextStepOnboarding(user_configuration.Step);
            user_configuration.Step = nextStep;

            user_configuration.TermCondition = term;

            _context.Configurations.Update(user_configuration);
            _context.SaveChanges();

            response.Result = user_configuration.TermCondition;
            return response;
        }

        public ApiResponse updateAutomaticStateConfiguration(string userId)
        {
            var response = new ApiResponse();
            var user_configuration = GetByUserId(userId);

            if (user_configuration == null)
            {
                response.AddError(
                    "El usuario no tiene configuracion asignada",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            ConfigurationsState nextStep = UserConfiguration.getStateConfiguration(
                user_configuration.State
            );
            user_configuration.State = nextStep;

            _context.Configurations.Update(user_configuration);
            _context.SaveChanges();

            response.Result = user_configuration.Step;
            return response;
        }
    }
}
