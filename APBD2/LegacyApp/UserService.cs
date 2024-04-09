using System;
using System.Runtime.CompilerServices;

namespace LegacyApp
{
    public class UserService 
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;

        public UserService() : this(new ClientRepository(), new UserCreditService()) {}

        private UserService(IClientRepository clientRepository, IUserCreditService userCreditService)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
        }

        private const string VeryImportantClient = "VeryImportantClient";
        private const string ImportantClient = "ImportantClient";

  
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUser(firstName, lastName, email, dateOfBirth, clientId))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);
            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            AssignCreditLimitToUser(user, client);

            if (!ValidateCreditLimit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
             return true;
        }

        private bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains('@') || !email.Contains('.'))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            return age >= 21;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private void AssignCreditLimitToUser(User user, Client client)
        {
            if (client.Type == VeryImportantClient)
            {
                user.HasCreditLimit = false;
            }
            else
            {
                var creditLimit = _userCreditService.GetCreditLimit(user.LastName);
                if (client.Type == ImportantClient)
                {
                    creditLimit *= 2;
                }

                user.CreditLimit = creditLimit;
                user.HasCreditLimit = true;
            }
        }

        private bool ValidateCreditLimit(User user)
        {
            return !(user.HasCreditLimit && user.CreditLimit < 500);
        }
    }
}