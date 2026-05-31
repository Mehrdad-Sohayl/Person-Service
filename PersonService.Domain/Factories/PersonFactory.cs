using PersonService.Domain.Entities;
using PersonService.Domain.Exceptions;
using PersonService.Domain.ValueObjects;

namespace PersonService.Domain.Factories
{
    public static class PersonFactory
    {
        public static Person Create(
            Guid? id,
            string firstName,
            string lastName,
            string nationalCode,
            DateTime birthDate)
        {
            var errors = new List<string>();

            Name? _firstName = null;
            Name? _lastName = null;
            NationalCode? _nationalCode = null;
            BirthDate? _birthDate = null;

            try { _firstName = new Name(firstName); }
            catch (ArgumentException ex) { errors.Add($"FirstName: {ex.Message}"); }

            try { _lastName = new Name(lastName); }
            catch (ArgumentException ex) { errors.Add($"LastName: {ex.Message}"); }

            try { _nationalCode = new NationalCode(nationalCode); }
            catch (ArgumentException ex) { errors.Add($"NationalCode: {ex.Message}"); }

            try { _birthDate = new BirthDate(birthDate); }
            catch (ArgumentException ex) { errors.Add($"BirthDate: {ex.Message}"); }

            if (errors.Any())
                throw new DomainValidationException(errors);

            return new Person(id, _firstName!, _lastName!, _nationalCode!, _birthDate!);
        }

        public static Person CreateForUpdate(
            Guid? id,
            string firstName,
            string lastName,
            DateTime birthDate)
        {
            var errors = new List<string>();

            Name? _firstName = null;
            Name? _lastName = null;
            NationalCode? _nationalCode = null;
            BirthDate? _birthDate = null;

            try { _firstName = new Name(firstName); }
            catch (ArgumentException ex) { errors.Add($"FirstName: {ex.Message}"); }

            try { _lastName = new Name(lastName); }
            catch (ArgumentException ex) { errors.Add($"LastName: {ex.Message}"); }

            try { _birthDate = new BirthDate(birthDate); }
            catch (ArgumentException ex) { errors.Add($"BirthDate: {ex.Message}"); }

            if (errors.Any())
                throw new DomainValidationException(errors);

            return new Person(id, _firstName!, _lastName!, _nationalCode!, _birthDate!);
        }
    }
}
