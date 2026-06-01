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
            var errors = new List<DomainError>();

            Name? _firstName = null;
            Name? _lastName = null;
            NationalCode? _nationalCode = null;
            BirthDate? _birthDate = null;

            try { _firstName = new Name(firstName); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            try { _lastName = new Name(lastName); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            try { _nationalCode = new NationalCode(nationalCode); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            try { _birthDate = new BirthDate(birthDate); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            if (errors.Any())
                throw new DomainValidationException(errors.ToList());

            return new Person(id, _firstName!, _lastName!, _nationalCode!, _birthDate!);
        }

        public static Person CreateForUpdate(
            Guid? id,
            string firstName,
            string lastName,
            DateTime birthDate)
        {
            var errors = new List<DomainError>();

            Name? _firstName = null;
            Name? _lastName = null;
            BirthDate? _birthDate = null;
            NationalCode? _nationalCode = null;


            try { _firstName = new Name(firstName); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            try { _lastName = new Name(lastName); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            try { _birthDate = new BirthDate(birthDate); }
            catch (DomainValidationException ex) { errors.AddRange(ex.Errors); }

            if (errors.Any())
                throw new DomainValidationException(errors.ToList());

            return new Person(id, _firstName!, _lastName!, _nationalCode!, _birthDate!);
        }
    }
}

