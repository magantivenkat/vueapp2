using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Controllers
{
    public class RegistrationTypeController : ProjectAdminControllerBase
    {
        private readonly IRegistrationTypeService _registrationTypeService;

        public RegistrationTypeController(IRegistrationTypeService registrationTypeService)
        {
            _registrationTypeService = registrationTypeService;
        }

        public async Task<IActionResult> Index(int projectId)
        {
            var listOfRegistrationTypeModels = await _registrationTypeService.GetAll(projectId);

            return View(listOfRegistrationTypeModels);
        }

        // GET: Admin/RegistrationType/Create
        public async Task<IActionResult> Create()
        {
            var model = new RegistrationTypeModel
            {
                RegistrationPaths = await _registrationTypeService.GetActiveRegPathSelectListAsync()
            };
            return View(model);
        }

        // POST: Admin/RegistrationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateValidation] // add this attribute instead of ModelState.Isvalid
        public async Task<IActionResult> Create(int projectId, RegistrationTypeModel registrationTypeModel)
        {
            TrimName(registrationTypeModel);
            if (await IsDuplicateName(projectId, registrationTypeModel.Name)) return View(registrationTypeModel);

            if (registrationTypeModel.RegistrationPathId == 0)
            {
                registrationTypeModel.RegistrationPathId = await _registrationTypeService.CreateRegPathAsync(registrationTypeModel);
                await _registrationTypeService.CreateAsync(projectId, registrationTypeModel, false);
                return RedirectToAction("Edit", "RegistrationPaths", new { id = registrationTypeModel.RegistrationPathId });
            }

            registrationTypeModel.RegistrationPathId = await _registrationTypeService.GetRegistrationPathId(projectId, registrationTypeModel.RegistrationPathId);
            await _registrationTypeService.CreateAsync(projectId, registrationTypeModel, false);
            return RedirectToAction(nameof(Index));
        }

        //[HttpGet("{id}")]
        // GET: Admin/RegistrationType/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var registrationTypeModel = await _registrationTypeService.GetById(id);

            if (registrationTypeModel == null)
            {
                return NotFound();
            }

            return View(registrationTypeModel);
        }

        // POST: Admin/RegistrationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateValidation]
        public async Task<IActionResult> Edit(int id, RegistrationTypeModel registrationTypeModel)
        {
            if (id != registrationTypeModel.Id)
            {
                return NotFound();
            }

            TrimName(registrationTypeModel);

            if (registrationTypeModel.Name != _registrationTypeService.GetById(id).Result.Name)
            {
                if (await IsDuplicateName(registrationTypeModel.ProjectId, registrationTypeModel.Name))
                {
                    return View(registrationTypeModel);
                }
            }

            await _registrationTypeService.UpdateAsync(registrationTypeModel);

            return RedirectToAction(nameof(Index));
        }

        //[HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var registrationTypeModel = await _registrationTypeService.GetById(id);

            if (registrationTypeModel == null)
            {
                return NotFound();
            }

            return View(registrationTypeModel);
        }

        // POST: Admin/RegistrationType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateValidation]
        public async Task<IActionResult> Delete(int id)
        {
            var registrationTypeModel = await _registrationTypeService.GetById(id);

            if (registrationTypeModel == null)
            {
                return NotFound();
            }

            await _registrationTypeService.DeleteAsync(registrationTypeModel.Id);

            return NoContent();
        }

        private static void TrimName(RegistrationTypeModel registrationTypeModel)
        {
            registrationTypeModel.Name = registrationTypeModel.Name.Trim();
        }

        private async Task<bool> IsDuplicateName(int projectId, string name)
        {
            if (await _registrationTypeService.IsDuplicateName(projectId, name))
            {
                ModelState.AddModelError("Name", "Duplicate name, please choose another name.");
                return true;
            }

            return false;
        }
    }
}
