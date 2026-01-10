using Microsoft.AspNetCore.Mvc;

namespace ResourceOne.Controllers
{
    public class ServicesController : Controller
    {
        [HttpGet]
        public IActionResult CompetencyAssessments()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PayrollOutsourcing()
        {
            return View();
        }
        [HttpGet]
        public IActionResult B2BTraining()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RecruitmentServices()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SalarySurveys()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GovernanceRestructuring()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TechnicalEducationInitiative()
        {
            return View();
        }
    }
}
