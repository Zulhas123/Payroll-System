using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Web.Controllers;

public sealed class DepartmentsController : Controller
{
    private readonly IRepository<Department> _departments;

    public DepartmentsController(IRepository<Department> departments)
    {
        _departments = departments;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _departments.ListAsync(cancellationToken);
        return View(items.OrderBy(d => d.Name).ToList());
    }

    public IActionResult Create() => View(new Department());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        await _departments.AddAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var item = await _departments.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Department model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        await _departments.UpdateAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var item = await _departments.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var item = await _departments.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();

        await _departments.DeleteAsync(item, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
