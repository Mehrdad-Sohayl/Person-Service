using Microsoft.AspNetCore.Mvc;
using PersonService.Client.Api.Models;
using PersonService.Client.Api.Services;
using PersonService.Contracts;

namespace PersonService.Client.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PersonsController : ControllerBase
    {
        private readonly CreatePersonService _createPersonService;
        private readonly UpdatePersonService _updatePersonService;
        private readonly DeletePersonService _deletePersonService;
        private readonly GetPersonService _getPersonService;

        public PersonsController(
            CreatePersonService createPersonService,
            UpdatePersonService updatePersonService,
            DeletePersonService deletePersonService,
            GetPersonService getPersonService)
        {
            _createPersonService = createPersonService;
            _updatePersonService = updatePersonService;
            _deletePersonService = deletePersonService;
            _getPersonService = getPersonService;
        }

        /// <summary>
        /// Creates a new person
        /// </summary>
        /// <param name="createPersonApiRequest">The person to create</param>
        /// <returns>The created person</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonResponse>> Create([FromBody] CreatePersonApiRequest createPersonApiRequest)
        {
            var createdPerson = await _createPersonService.CreateAsync(createPersonApiRequest);
            return Ok(createdPerson);
        }

        /// <summary>
        /// Gets a person by ID
        /// </summary>
        /// <param name="getPersonByIdApiRequest">The person ID</param>
        /// <returns>The person</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> Get(string id)
        {
            try
            {
                var person = await _getPersonService.GetAsync(id);
                return Ok(person);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates a person's first name
        /// </summary>
        /// <param name="updatePersonApiRequest">The updated person data</param>
        /// <returns>The updated person</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> UpdateFirstName([FromBody] UpdatePersonApiRequest updatePersonApiRequest)
        {
            try
            {
                var updatedPerson = await _updatePersonService.UpdateFirstNameAsync(updatePersonApiRequest);
                return Ok(updatedPerson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates a person's last name
        /// </summary>
        /// <param name="updatePersonApiRequest">The updated person data</param>
        /// <returns>The updated person</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> UpdateLastName([FromBody] UpdatePersonApiRequest updatePersonApiRequest)
        {
            try
            {
                var updatedPerson = await _updatePersonService.UpdateLastNameAsync(updatePersonApiRequest);
                return Ok(updatedPerson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates a person's birth date
        /// </summary>
        /// <param name="updatePersonApiRequest">The updated person data</param>
        /// <returns>The updated person</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> UpdateBirthDate([FromBody] UpdatePersonApiRequest updatePersonApiRequest)
        {
            try
            {
                var updatedPerson = await _updatePersonService.UpdateBirthDateAsync(updatePersonApiRequest);
                return Ok(updatedPerson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a person
        /// </summary>
        /// <param name="id">The person ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _deletePersonService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}