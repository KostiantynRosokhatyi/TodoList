using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using Users.Data;
using TodoList.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TodoList.Controllers
{
    [Authorize]
    public class TodoItemsController : Controller
    {
        private readonly TodoListContext _context;
        private readonly UserListContext _contextUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TodoItemsController(TodoListContext context, UserListContext contextUser, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextUser = contextUser;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var todoListContext = _contextUser.TodoItem.Include(t => t.User).Where(t => t.User.Id == userId);
            return View(await todoListContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _contextUser.TodoItem
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemName, ItemDate, IsItemCompleted")] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                var userId = this.GetCurrentUserId();

                var user = await _contextUser.User.FirstOrDefaultAsync(x => x.Id == userId);

                if (user != null)
                {

                    todoItem.UserId = userId;

                    _contextUser.Add(todoItem);
                    await _contextUser.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }


            return View(todoItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _contextUser.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return View(todoItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,ItemDate,IsItemCompleted")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    var existingTodoItem = await _contextUser.TodoItem.FindAsync(id);

                    if (existingTodoItem == null)
                    {
                        return NotFound();
                    }

                    existingTodoItem.ItemName = todoItem.ItemName;
                    existingTodoItem.ItemDate = todoItem.ItemDate;
                    existingTodoItem.IsItemCompleted = todoItem.IsItemCompleted;

                    _contextUser.Update(existingTodoItem);
                    await _contextUser.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _contextUser.TodoItem
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoItem = await _contextUser.TodoItem.FindAsync(id);
            if (todoItem != null)
            {
                _contextUser.TodoItem.Remove(todoItem);
                await _contextUser.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst("Id");

            if (claim == null)
            {

                throw new Exception("User id does not exist or");
            }

            return int.Parse(claim.Value);
        }

        private bool TodoItemExists(int id)
        {
            return _contextUser.TodoItem.Any(e => e.Id == id);
        }
    }
}
