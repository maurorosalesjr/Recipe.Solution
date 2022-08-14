using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Recipe.Controllers
{
  [Authorize]
  public class FormulasController : Controller
  {
    private readonly RecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FormulasController(UserManager<ApplicationUser> userManager, RecipeContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userFormulas = _db.Formulas.Where(entry => entry.User.Id == currentUser.Id).ToList();
        return View(userFormulas);
    }

    public ActionResult Create()
    {
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "TagName");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Formula formula, int TagId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        formula.User = currentUser;
        _db.Formulas.Add(formula);
        _db.SaveChanges();
        if (TagId != 0)
        {
            _db.Box.Add(new Box() { TagId = TagId, FormulaId = formula.FormulaId });
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisFormula = _db.Formulas
        .Include(formula => formula.JoinEntities)
        .ThenInclude(join => join.Tag)
        .FirstOrDefault(formula => formula.FormulaId == id);
      return View(thisFormula);
    }

    public ActionResult Edit(int id)
    {
        var thisFormula = _db.Formulas.FirstOrDefault(formula => formula.FormulaId == id);
        ViewBag.TagId = new SelectList(_db.Tags, "TagId", "TagName");
        return View(thisFormula);
    }

    [HttpPost]
    public ActionResult Edit(Formula formula, int TagId)
    {
      if (TagId != 0)
      {
        _db.Box.Add(new Box() { TagId = TagId, FormulaId = formula.FormulaId });
      }
      _db.Entry(formula).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTag(int id)
    {
        var thisFormula = _db.Formulas.FirstOrDefault(formula => formula.FormulaId == id);
        ViewBag.TagId = new SelectList(_db.Tags, "TagId", "TagName");
        return View(thisFormula);
    }

    [HttpPost]
    public ActionResult AddTag(Formula formula, int TagId)
    {
        if (TagId != 0)
        {
          _db.Box.Add(new Box() { TagId = TagId, FormulaId = formula.FormulaId });
          _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisFormula = _db.Formulas.FirstOrDefault(formula => formula.FormulaId == id);
      return View(thisFormula);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisFormula = _db.Formulas.FirstOrDefault(formula => formula.FormulaId == id);
      _db.Formulas.Remove(thisFormula);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteTag(int joinId)
    {
        var joinEntry = _db.Box.FirstOrDefault(entry => entry.BoxId == joinId);
        _db.Box.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
  }
}