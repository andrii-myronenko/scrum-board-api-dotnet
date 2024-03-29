using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using ScrumBoardAPI.Core.Models.Paging;
using ScrumBoardAPI.Core.Models.Workspace;

namespace ScrumBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Administrator")]
    [ApiController]
    public class WorkspaceController : BaseApplicationController
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IUserRepository _userRepository;

        public WorkspaceController(IWorkspaceRepository workspaceRepository, IUserRepository userRepository, IMapper autoMapper)
        {
            this._workspaceRepository = workspaceRepository;
            this._userRepository = userRepository;
        }

        // GET: api/Workspace/OData
        [HttpGet("OData")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetWorkspaceDto>>> GetWorkspace()
        {
            var userId = GetUserId();
            var workspaces = await _workspaceRepository.GetWorkspacesByUserId<GetWorkspaceDto>(userId);

            return workspaces;
        }

        // GET: api/Workspace/?PageSize=25&PageNumber=1
        [HttpGet()]
        public async Task<ActionResult<PagedResult<GetWorkspaceDto>>> GetPagedWorkspaces([FromQuery] QueryParameters queryParameters)
        {
            var userId = GetUserId();
            var workspaces = await _workspaceRepository.GetPagedWorkspacesByUserId<GetWorkspaceDto>(userId, queryParameters);

            return workspaces;
        }

        // GET: api/Workspace/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetWorkspaceDetailsDto>> GetWorkspace(int id)
        {
            var userId = GetUserId();

            var canUserAccessWorkspace = await _userRepository.CanUserAccessWorkspace(userId, id);

            if (!canUserAccessWorkspace) {
                return Forbid();
            }

            var workspace = await _workspaceRepository.GetWorkspaceWithDetails<GetWorkspaceDetailsDto>(id);

            if (workspace == null)
            {
                return NotFound();
            }

            return workspace;
        }

        // PUT: api/Workspace/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> RenameWorkspace(int id, string newName)
        {
            var userId = GetUserId();

            var isWorkspaceAdmin = await _userRepository.IsUserWorkspaceAdmin(userId, id);

            if (!isWorkspaceAdmin) {
                return Forbid();
            }

            try
            {
                await _workspaceRepository.RenameWorkspace(id, newName);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkspaceExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Workspace/5/AddUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/AddUser")]
        public async Task<IActionResult> AddUserToWorkspace(int id, string userName)
        {
            var userId = GetUserId();

            var isWorkspaceAdmin = await _userRepository.IsUserWorkspaceAdmin(userId, id);

            if (!isWorkspaceAdmin) {
                return Forbid();
            }

            try
            {
                await _workspaceRepository.AddUserToWorkspace(id, userName);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkspaceExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{id}/RemoveUser")]
        public async Task<IActionResult> RemoveUserFromWorkspace(int id, string userName)
        {
            var userId = GetUserId();

            var isWorkspaceAdmin = await _userRepository.IsUserWorkspaceAdmin(userId, id);

            if (!isWorkspaceAdmin) {
                return Forbid();
            }

            try
            {
                await _workspaceRepository.RemoveUserFromWorkspace(id, userName);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkspaceExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Workspace
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetWorkspaceDto>> PostWorkspace(string name)
        {
            var creatorId = GetUserId();
            var workspace = await _workspaceRepository.CreateWorkspace<GetWorkspaceDto>(name, creatorId);
            return CreatedAtAction("GetWorkspace", new { id = workspace.Id }, workspace);
        }

        // DELETE: api/Workspace/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkspace(int id)
        {
            var userId = GetUserId();

            var isWorkspaceAdmin = await _userRepository.IsUserWorkspaceAdmin(userId, id);

            if (!isWorkspaceAdmin) {
                return Forbid();
            }

            var workspaceExists = await WorkspaceExistsAsync(id);

            if (!workspaceExists)
            {
                return NotFound();
            }

            await _workspaceRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> WorkspaceExistsAsync(int id)
        {
            return await _workspaceRepository.Exists(id);
        }
    }
}
