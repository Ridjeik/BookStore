using BLL.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly IDataSetsService _dataSetsService;
    private readonly IEmployeeService _employeeService;

    public AdminController(IDataSetsService dataSetsService, IEmployeeService employeeService)
    {
        _dataSetsService = dataSetsService;
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployees();
        return View(employees);
    }

    [HttpGet("CreateDataSet")]
    public async Task<IActionResult> CreateDataSet()
    {
        var result = await _dataSetsService.CreateDataSet();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("DeleteDataSet")]
    public async Task<IActionResult> DeleteDataSet()
    {
        var result = await _dataSetsService.ClearDatabaseAsync();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    // Employee methods



    public async Task<IActionResult> EmployeeDetails(string id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        return View(employee);
    }

    public IActionResult CreateEmployee()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.CreateEmployee(employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    public async Task<IActionResult> EditEmployee(string id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmployee(string id, Employee employee)
    {
        if (id != employee.UserId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _employeeService.UpdateEmployee(employee);
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }

    public async Task<IActionResult> DeleteEmployee(string id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        return View(employee);
    }

    [HttpPost, ActionName("DeleteEmployee")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
    {
        await _employeeService.DeleteEmployee(id);
        return RedirectToAction(nameof(Index));
    }
}
