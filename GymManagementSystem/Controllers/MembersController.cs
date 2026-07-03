using GymManagementSystem.BLL.Services.Attachment;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace GymManagementSystem.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MembersController : Controller
    {

        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(
            IMemberService memberService,
            IAttachmentService attachmentService
            )
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Picture(int id, CancellationToken ct = default)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member is null || string.IsNullOrWhiteSpace(member.Photo)) return NotFound();
            var result = _attachmentService.GetFile("MemberPicture", member.Photo);
            if(result is null) return NotFound();
            return File(result.Value.stream,result.Value.contentType);
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);

            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.CreateMemberAsync(model, ct);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Member created successfully !";
                }
                else
                {
                    TempData["ErrorMessage"] = result.error;                                                                                                              
                }

                return RedirectToAction("Index");

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {

            var result = await _memberService.GetMemberDetailsAsync(id, ct);
            if(result is null)
            {
                TempData["ErrorMessage"] = "Member not found !";
                return RedirectToAction("Index");
            }

            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Health Record not found !!";
                return RedirectToAction("Index");
            }
            return View(result); 
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
           var result = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Edit Member not found !!";
                return RedirectToAction("Index");
            }

            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(id,model, ct);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Member Update successfully !";
                }
                else
                {
                    TempData["ErrorMessage"] = result.error;
                }

                return RedirectToAction("Index");

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberDetailsAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Delete Member not found !!";
                return RedirectToAction("Index");
            }
            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully :)";
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }
            return RedirectToAction("Index");
        }


    }
}
